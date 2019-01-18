using System;
using Gu.Wpf.UiAutomation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testio.Tests
{
    [TestClass]
    public class TestioShould
    {
        private const string ExeFileName = "Testio.exe";

        [TestInitialize]
        public void TestInitialize()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName))
            {
                Wait.UntilInputIsProcessed();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestMethod]
        public void DisableLoginButton_WhenUsernameAndPasswordEmpty()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName))
            {
                var window = app.MainWindow;
                var username = window.FindTextBox("UserName");
                var password = window.FindPasswordBox("Password");
                var loginButton = window.FindButton("Login");

                username.Text = string.Empty;
                password.SetValue(string.Empty);
                Assert.IsFalse(loginButton.IsEnabled);
            }
        }

        [TestMethod]
        public void ShouldShowMessageBox_WhenUsernameOrPasswordIsIncorrect()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName))
            {
                var window = app.MainWindow;
                var username = window.FindTextBox("UserName");
                var password = window.FindPasswordBox("Password");
                var loginButton = window.FindButton("Login");

                username.Text = "test123";
                password.SetValue("password");
                loginButton.Click(true);

                MessageBox errorBox = null;
                for (int i = 0; i < 5; i++)
                {
                    Wait.For(TimeSpan.FromSeconds(1));
                    errorBox = window.FindMessageBox();
                    if (errorBox != null)
                    {
                        break;
                    }
                }

                Assert.AreEqual("Unable to login", errorBox?.Caption);

            }
        }

        [TestMethod]
        public void ShouldShowLogOutButton_WhenUsernameAndPasswordAreCorrect()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName))
            {
                var window = app.MainWindow;
                var username = window.FindTextBox("UserName");
                var password = window.FindPasswordBox("Password");
                var loginButton = window.FindButton("Login");

                username.Text = "tesonet";
                password.SetValue("partyanimal");
                loginButton.Click(true);

                Button logoutButton = null;
                for (int i = 0; i < 10; i++)
                {
                    Wait.For(TimeSpan.FromSeconds(1));
                    try
                    {
                        logoutButton = window.FindButton("Logout");
                        if (logoutButton != null)
                        {
                            break;
                        }
                    }
                    catch { }
                }

                Assert.IsNotNull(logoutButton);

            }
        }
    }
}
