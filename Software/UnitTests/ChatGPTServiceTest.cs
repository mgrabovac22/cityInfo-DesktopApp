using BusinessLogicLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class ChatGPTServiceTest
    {
        [TestMethod]
        public void Constructor_PassTheApiKeyAndItIsSavedInVariable_ValidApiKey()
        {
            var apiKey = "idawklawdkjlawdkaw=oipdqdw_aow?oawdoja";
            ChatGPTService chatGPTService = new ChatGPTService(apiKey);

            FieldInfo fieldInfo = typeof(ChatGPTService).GetField("_apiKey", BindingFlags.NonPublic | BindingFlags.Instance);
            var value = (string)fieldInfo.GetValue(chatGPTService);

            Assert.AreEqual(apiKey, value);
        }

        [TestMethod]
        public async Task GetResponseAsync_PassThePromptAndGetAValidStatusAndResponseFromApiKey_ResponseNotToBeNull()
        {
            var apiKey = "idawklawdkjlawdkaw=oipdqdw_aow?oawdoja";
            var prompt = "Kako si mi dragi?";
            ChatGPTService chatGPTService = new ChatGPTService(apiKey);

            var response = await chatGPTService.GetResponseAsync(prompt);

            Assert.IsNotNull(response);
        }
    }
}
