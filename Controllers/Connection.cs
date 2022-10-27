using Microsoft.AspNetCore.Mvc;
using NLog;
using SshService.Shell;

namespace SshService.Controllers;

public class RequestParam
{
    public int Port { get; set; }
    public string IpAddress { get; set; } = string.Empty;
}

[Route("api")]
[ApiController]
public class Connection : ControllerBase
{
    private static Logger logger = LogManager.GetCurrentClassLogger();

    private string KeyLocation
    {
        get
        {
            var keyDir = Path.Join(Common.BasePath, "ssh-key");
            if (!Directory.Exists(keyDir))
            {
                Directory.CreateDirectory(keyDir);
            }
            return Path.Join(keyDir, "key");
        }
    }

    [Route("AddRule")]
    [HttpPost]
    public FirewallResult AddRule([FromForm] RequestParam param)
    {
        return Firewall.Add(param.IpAddress, param.Port); ;
    }

    [Route("DeleteRule")]
    [HttpPost]
    public FirewallResult DeleteRule([FromForm] RequestParam param)
    {
        return Firewall.Remove(param.Port);
    }

    [Route("Status")]
    [HttpGet]
    public FirewallResult Status()
    {
        return Firewall.Status();
    }

    [Route("GenerateKey")]
    [HttpGet]
    public Dictionary<string, object> GenerateKey()
    {
        var result = new Dictionary<string, object>();
        result["Success"] = false;
        try
        {
            bool isKeyExists = System.IO.File.Exists(KeyLocation);
            var cmdConfirmationArgs = isKeyExists ? "-y" : "";
            var commandResult = ShellUtility.Run(@"sudo", $"ssh-keygen -m pem -t rsa -b 2048 -C \"\" -N \"\" -f {KeyLocation} {cmdConfirmationArgs}");
            result["Message"] = commandResult.Result;

            var pubKey = $"{KeyLocation}.pub";
            var priKey = KeyLocation;

            if (!isKeyExists)
            {
                result["Message"] = "SSH Keys not generated";
                return result;
            }

            System.IO.File.WriteAllText(Common.AuthorizedKeysFilePath, System.IO.File.ReadAllText(pubKey));
            result["KeyContent"] = System.IO.File.ReadAllText(priKey);
            result["Success"] = true;
        }
        catch (Exception ex)
        {
            result["Message"] = ex.Message;
            result["Success"] = false;
            logger.Error(ex);
        }
        return result;
    }

    [Route("GetSshKey")]
    [HttpGet]
    public Dictionary<string, object> GetSshKey()
    {
        var result = new Dictionary<string, object>();
        result["Success"] = false;
        try
        {
            var pubKey = $"{KeyLocation}.pub";
            var priKey = KeyLocation;

            if (!System.IO.File.Exists(KeyLocation))
            {
                result["Message"] = "SSH Keys not exists";
                return result;
            }

            result["KeyContent"] = System.IO.File.ReadAllText(priKey);
            result["Success"] = true;
        }
        catch (Exception ex)
        {
            result["Message"] = ex.Message;
            result["Success"] = false;
            logger.Error(ex);
        }
        return result;
    }


    [Route("Test")]
    [HttpPost]
    [HttpGet]
    public string Test()
    {
        return "Working: Testing API";
    }
}