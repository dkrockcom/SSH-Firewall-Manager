using NLog;
using SshService.Shell;
using System.Text.RegularExpressions;

namespace SshService
{
    public static class Firewall
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static FirewallResult Add(string ip, int port)
        {
            var result = new FirewallResult();
            try
            {
                var args = $"ufw allow from {ip} to any port {port}";
                logger.Debug($"Add Rule: {args}");
                var commandResult = ShellUtility.Run("sudo", args);
                logger.Debug($"Add Rule Command Response", commandResult);
                result.Message = commandResult.Result;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                logger.Error(ex);
            }
            return result;
        }

        public static FirewallResult Remove(int port)
        {
            var result = new FirewallResult();
            try
            {
                DeleteRule(port);
                result.Message = "Rule Deleted";
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                logger.Error(ex);
            }
            return result;
        }

        private static void DeleteRule(int port)
        {
            var num = GetRuleNumber(port);
            if (num != 0)
            {
                logger.Debug($"DELETE Firewall Rule RuleId: {num}");
                var fileName = Path.Join(Common.BasePath, "removeRule.sh");
                var result = ShellUtility.Run("bash", $"{fileName} {num}");
                logger.Debug($"DeleteRule Command Response", result);
                DeleteRule(port);
            }
        }

        private static int GetRuleNumber(int portNumber)
        {
            var ruleNumber = 0;
            try
            {
                var commandResult = ShellUtility.Run("sudo", "ufw status numbered");
                var splittedLines = commandResult.Result.Split("\n");
                for (int i = 0; i < splittedLines.Length; i++)
                {
                    var line = splittedLines[i];
                    if (line.IndexOf(portNumber.ToString()) > -1)
                    {
                        var regex = new Regex(@"\[(.*?)\]");
                        Match m = regex.Match(line);
                        if (m.Success)
                        {
                            ruleNumber = Convert.ToInt32(m.Groups[1].Value);
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ruleNumber = 0;
            }
            return ruleNumber;
        }

        public static FirewallResult Status()
        {
            var result = new FirewallResult();
            try
            {
                var commandResult = ShellUtility.Run("sudo", $"ufw status numbered");
                result.Message = commandResult.Result;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                logger.Error(ex);
            }
            return result;
        }
    }
}