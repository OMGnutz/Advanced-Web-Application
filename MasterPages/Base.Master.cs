using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Mail;
using Salt_Password;
using System.Diagnostics;
using System.Xml.Linq;
using Org.BouncyCastle.Crypto;
using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Web.Optimization;
using System.Web.Mvc;
using System.Threading;



namespace _211792H.MasterPages
{
    public partial class Base : System.Web.UI.MasterPage
    {
        public static string tempemail;
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString;
        private static string emailLink;
        private static string fpemail;
        protected void Page_Load(object sender, EventArgs e)
        {
           

        }

 
        protected void VerifyEmail(object sender, EventArgs e)
        {
            RegistrationComplete.Style.Add("display", "none");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            bool exists = false;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@Email", txt_Email.Text);
                exists = (int)cmd.ExecuteScalar() > 0;
            }

            if (exists)
            {
                SignupPopup.Style.Add("display", "block");
                EmailExist.Style.Add("display", "block");
                txt_CEmail.Attributes["class"] = "form-control is-invalid";
            }

            else
            {
                SignupPopup.Style.Add("display", "none");
                LoadingScreen.Style.Add("display", "block");
                try
                {
                    Debug.WriteLine("huh");
                    Guid newGUID = Guid.NewGuid();
                    emailLink = newGUID.ToString();
                    MailMessage newMail = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    newMail.From = new MailAddress("vannsar04@gmail.com", "Test");
                    newMail.To.Add(txt_Email.Text);
                    newMail.IsBodyHtml = true;
                    newMail.Body = @"<!DOCTYPE html>
                                <html>
                                <head>
                                    <meta charset=""utf-8"" />
                                    <title>Email Verification</title>
                                </head>
                                <body>
                                <b>Please click on the button below to verify your email</b>
                                <br/> 
                                </body>
                                </html>";
                    newMail.Body += "<a href = 'https://localhost:44320/WebPages/EmailVerified.aspx?token=" + emailLink + "'>VERIFY EMAIL ADDRESS</a>";
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("vannsar04@gmail.com", "txirfuzojmaguwmz");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    tempemail = txt_Email.Text;
                    client.Send(newMail);
                    string dropTable = "DROP TABLE IF EXISTS [dbo].[TempUser]";
                    SqlCommand dropTempUser = new SqlCommand(dropTable, conn);
                    dropTempUser.ExecuteNonQuery();
                    string createTable = "CREATE TABLE [dbo].[TempUser](Email nvarchar(MAX) , Verified bit , EmailLinkUsed nvarchar(MAX) , EmailSentTime datetime)";
                    SqlCommand createcmd = new SqlCommand(createTable, conn);
                    createcmd.ExecuteNonQuery();
                    string insertValue = "INSERT INTO [TempUser](Email , verified , EmailLinkUsed , EmailSentTime) values (@email , @verified , @emailID , @time)";
                    SqlCommand insertcmd = new SqlCommand(insertValue, conn);
                    insertcmd.Parameters.AddWithValue("@email", txt_Email.Text);
                    insertcmd.Parameters.AddWithValue("@verified", false);
                    insertcmd.Parameters.AddWithValue("@emailID", emailLink);
                    insertcmd.Parameters.AddWithValue("@time", DateTime.UtcNow);
                    insertcmd.ExecuteNonQuery();
                    startDepend();
                }


                catch (SmtpException ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                catch (SqlException odbcEx)
                {
                    Console.WriteLine(odbcEx.ToString());
                }


                finally
                {
                    conn.Close();
                }
            }

        }



        protected void startDepend()
        {
            string verified = "SELECT Verified FROM [TempUser] WHERE Email=@Email";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            SqlCommand verifiedcmd = new SqlCommand(verified, conn);
            verifiedcmd.Parameters.AddWithValue("@Email", txt_Email.Text);
            SqlDependency.Start(_connectionString);
            SqlDependency dependency = new SqlDependency(verifiedcmd);
            dependency.OnChange -= new OnChangeEventHandler(Emailverified);
            dependency.OnChange += new OnChangeEventHandler(Emailverified);
            verifiedcmd.ExecuteReader();
            conn.Close();
        }

        protected void stopDepend()
        {
            SqlDependency.Stop(_connectionString);
        }



        protected void Emailverified(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            try
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT Verified FROM [TempUser] WHERE Email=@Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", tempemail);
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (Convert.ToBoolean(reader["Verified"]) == true)
                    {
                        TriggerHub.sendMessage();
                        txt_CEmail.Text = "";
                        txt_Email.Text = "";
                        Debug.WriteLine(LoadingScreen.Style["display"].ToString());
                        Debug.WriteLine(tempemail);
                        stopDepend();

                    }
                    else
                    {
                        startDepend();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error -" + ex);
            }
            finally
            {
                conn.Close();
            }
        }

        protected void AccCreation(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            LoadingScreen.Style.Add("display", "none");
            Guid newGUID = Guid.NewGuid();
            string ePass = Hash.ComputeHash(txt_password.Text, "SHA512", null);
            conn.Open();
            string insertAcc = "INSERT INTO [User] (Id , Username , Displayname , Email , Password , AccLocked) values (@id , @username , @displayname , @email , @password, @status)";
            SqlCommand insertcmd = new SqlCommand(insertAcc, conn);
            insertcmd.Parameters.AddWithValue("@id", newGUID.ToString());
            insertcmd.Parameters.AddWithValue("@username", txt_username.Text);
            insertcmd.Parameters.AddWithValue("@displayname", txt_username.Text);
            insertcmd.Parameters.AddWithValue("@email", tempemail);
            insertcmd.Parameters.AddWithValue("@password", ePass);
            insertcmd.Parameters.AddWithValue("@status", false);
            insertcmd.ExecuteNonQuery();
            conn.Close();
            txt_password.Text = "";
            txt_username.Text = "";
            txt_Cpassword.Text = "";
            tempemail = txt_Email.Text;
            RegistrationComplete.Style.Add("display", "block");
        }


        protected void AccLogin(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            try
            {
                conn.Open();
                SqlCommand cmdUsername = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE Username = @username", conn);
                cmdUsername.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                int temp = Convert.ToInt32(cmdUsername.ExecuteScalar().ToString());
                conn.Close();
                if (temp == 1)
                {
                    Account_LockoutExpire(txt_usernameLogin.Text);
                    conn.Open();
                    string passCheck = "SELECT Password FROM [User] WHERE Username = @username";
                    SqlCommand passcmd = new SqlCommand(passCheck, conn);
                    passcmd.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                    string userpassword = passcmd.ExecuteScalar().ToString();
                    bool flag = Hash.VerifyHash(txt_passwordLogin.Text, "SHA512", userpassword);
                    string chkstatus = "SELECT AccLocked FROM [User] WHERE Username = @username";
                    SqlCommand chkstatuscmd = new SqlCommand(chkstatus, conn);
                    chkstatuscmd.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                    if (flag == true && Convert.ToBoolean(chkstatuscmd.ExecuteScalar().ToString()) == false)
                    {
                        string getdisplayname = "SELECT Displayname FROM [User] WHERE Username=@username";
                        SqlCommand cmddisplay = new SqlCommand(getdisplayname, conn);
                        cmddisplay.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                        SqlDataReader reader= cmddisplay.ExecuteReader();
                        reader.Read();
                        Session["CHANGE_MASTERPAGE"] = "~/MasterPages/AfterLogin.Master";
                        Session["CHANGE_MASTERPAGE2"] = null;
                        Session["username"] = reader["Displayname"].ToString();
                        reader.Close();
                        Response.Redirect(Request.Url.AbsoluteUri, false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        AccountLoginAttempts(txt_usernameLogin.Text);
                        LoginPopUp.Style.Add("display", "block");
                        Wrongcredentials.Style.Add("display", "block");
                    }
                    conn.Close();
                    Account_Lockout(txt_usernameLogin.Text);
                    
                }
                else
                {
                    conn.Open();
                    SqlCommand cmdUsernameAdmin = new SqlCommand("SELECT COUNT(*) FROM [Admin] WHERE Username = @username", conn);
                    cmdUsernameAdmin.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                    int tempAdmin = Convert.ToInt32(cmdUsernameAdmin.ExecuteScalar().ToString());
                    if (tempAdmin == 1)
                    {
                        string passCheckAdmin = "SELECT Password FROM [Admin] WHERE Username = @username";
                        SqlCommand passAdmincmd = new SqlCommand(passCheckAdmin, conn);
                        passAdmincmd.Parameters.AddWithValue("@username", txt_usernameLogin.Text);
                        string adminpassword = passAdmincmd.ExecuteScalar().ToString();
                        bool flagAdmin = Hash.VerifyHash(txt_passwordLogin.Text, "SHA512", adminpassword);
                        if (flagAdmin == true)
                        {
                            Session["username"] = txt_usernameLogin.Text;
                            Response.Redirect("../WebPages/Admin/Dashboard.aspx");
                        }
                        else
                        {
                            LoginPopUp.Style.Add("display", "block");
                            Wrongcredentials.Style.Add("display", "block");
                        }
                    }

                    else
                    {
                        LoginPopUp.Style.Add("display", "block");
                        Wrongcredentials.Style.Add("display", "block");
                    }
                    conn.Close();
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }

            finally
            {
                txt_passwordLogin.Text = "";
                txt_usernameLogin.Text = "";
                conn.Close();
            }
        }


        protected void Forgetpass(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            try
            {
                conn.Open();
                string searchuser = "SELECT * FROM [User] WHERE Username = @search or Email = @search";
                SqlCommand searchcmd = new SqlCommand(searchuser, conn);
                searchcmd.Parameters.AddWithValue("@search", txt_ForgetPass.Text);
                SqlDataReader reader = searchcmd.ExecuteReader();
                if (reader.Read())
                {
                    Guid newGUID = Guid.NewGuid();
                    emailLink = newGUID.ToString();
                    MailMessage newMail = new MailMessage();
                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    newMail.From = new MailAddress("vannsar04@gmail.com", "Test");
                    newMail.To.Add(reader["Email"].ToString());
                    newMail.IsBodyHtml = true;
                    newMail.Body = @"<!DOCTYPE html>
                                <html>
                                <head>
                                    <meta charset=""utf-8"" />
                                    <title>Forget Password</title>
                                </head>
                                <body>
                                <b>Please click on the button below to change your password</b>
                                <br/> 
                                </body>
                                </html>";
                    newMail.Body += "<a href = 'https://localhost:44320/WebPages/ForgetPassword.aspx?token=" + emailLink + "'>Change Password</a>";
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("vannsar04@gmail.com", "txirfuzojmaguwmz");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(newMail);
                    fpemail = reader["email"].ToString();
                    reader.Close();
                    sendtemporaryEmail(emailLink , fpemail);
                    ForgetPasswordPopUp.Style.Add("display", "none");
                    PRalert.Style.Add("display", "block");
                    txt_ForgetPass.Text = "";
                }

                else
                {
                    ForgetPasswordPopUp.Style.Add("display", "block");
                    NotExist.Style.Add("display", "block");
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally { conn.Close(); }
        }


        protected void sendtemporaryEmail(string emailID , string email)
        {
            Debug.WriteLine("error here");
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string previousemailexist = "SELECT * FROM [TempEmail] WHERE Email=@email";
            SqlCommand previousemailcmd = new SqlCommand(previousemailexist, conn);
            previousemailcmd.Parameters.AddWithValue("@email", email);
            SqlDataReader reader = previousemailcmd.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                string deleteprevious = "DELETE FROM [TempEmail] WHERE Email=@email";
                SqlCommand deletepreviouscmd = new SqlCommand(deleteprevious, conn);
                deletepreviouscmd.Parameters.AddWithValue("@email", email);
                deletepreviouscmd.ExecuteNonQuery();
            }

            string insertValue = "INSERT INTO [TempEmail](EmailCode, EmailSentTime, email) values (@emailID , @time , @email)";
            SqlCommand insertcmd = new SqlCommand(insertValue, conn);
            insertcmd.Parameters.AddWithValue("@emailID", emailID);
            insertcmd.Parameters.AddWithValue("@time", DateTime.UtcNow);
            insertcmd.Parameters.AddWithValue("@email", email);
            insertcmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void validateEmailTime(string emailID)
        {
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan duration = TimeSpan.FromMinutes(10);
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string expire = "SELECT EmailSentTime From [TempEmail] WHERE EmailCode=@emailid";
            SqlCommand expirecmd = new SqlCommand(expire, conn);
            expirecmd.Parameters.AddWithValue("@emailid", emailID);
            SqlDataReader reader = expirecmd.ExecuteReader();
            if (reader.Read())
            {
                if (currentTime - Convert.ToDateTime(reader["EmailSentTime"]) >= duration)
                {
                    reader.Close();
                    string linkexpired = "DELETE FROM [TempEmail] WHERE EmailCode=@emailid";
                    SqlCommand linkexpiredcmd = new SqlCommand(linkexpired, conn);
                    linkexpiredcmd.Parameters.AddWithValue("@emailid", emailID);
                    linkexpiredcmd.ExecuteNonQuery();
                }
                else
                {
                    return;
                }
            }
            conn.Close();
        }


        protected void Account_Lockout(string username)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string selectattempt = "SELECT LoginAttempts , AccLocked FROM [User] WHERE Username=@username";
            SqlCommand selectattemptcmd = new SqlCommand(selectattempt , conn);
            selectattemptcmd.Parameters.AddWithValue("@username", username);
            SqlDataReader attemptread = selectattemptcmd.ExecuteReader();
            attemptread.Read();
            if (attemptread["LoginAttempts"].ToString() != "" && attemptread["AccLocked"].ToString() != "")
            {
                if (Convert.ToInt32(attemptread["LoginAttempts"].ToString()) >= 5 && Convert.ToBoolean(attemptread["AccLocked"].ToString()) != true)
                {
                    attemptread.Close();
                    string lockuser = "UPDATE [USER] SET AccLocked=@status , AccLockedTime=@time WHERE Username=@username";
                    SqlCommand lockusercmd = new SqlCommand(lockuser, conn);
                    lockusercmd.Parameters.AddWithValue("@status", true);
                    lockusercmd.Parameters.AddWithValue("@time", DateTime.UtcNow);
                    lockusercmd.Parameters.AddWithValue("@username", username);
                    lockusercmd.ExecuteNonQuery();

                }

                else if (Convert.ToBoolean(attemptread["AccLocked"].ToString()) == true)
                {
                    Debug.WriteLine("here");
                    Wrongcredentials.Style.Add("display", "none");
                    TooManyAttempts.Style.Add("display", "block");
                }

                else if(Convert.ToBoolean(attemptread["AccLocked"].ToString()) == false)
                {
                    TooManyAttempts.Style.Add("display", "none");
                }
            }

            conn.Close();
        }

        protected void Account_LockoutExpire(string username)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            Debug.WriteLine(username);
            string selectTime = "SELECT * FROM [User] WHERE Username=@username";
            SqlCommand selectTimecmd = new SqlCommand(selectTime, conn);
            selectTimecmd.Parameters.AddWithValue("@username", username);
            SqlDataReader timereader = selectTimecmd.ExecuteReader();
            timereader.Read();
            if (timereader["AccLockedTime"].ToString() != "")
            {
                DateTime currentTime = DateTime.UtcNow;
                TimeSpan duration = TimeSpan.FromMinutes(10);
                if (currentTime - Convert.ToDateTime(timereader["AccLockedTime"]) >= duration)
                {
                    timereader.Close();
                    string reset = "UPDATE [User] SET LoginAttempts=@attempt , AccLocked=@status , AccLockedTime=NULL WHERE Username=@username";
                    SqlCommand resetcmd = new SqlCommand(reset, conn);
                    resetcmd.Parameters.AddWithValue("@attempt", 0);
                    resetcmd.Parameters.AddWithValue("@status", false);
                    resetcmd.Parameters.AddWithValue("@username", username);
                    resetcmd.ExecuteNonQuery();
                }
            }     
            conn.Close();
        }

        protected void AccountLoginAttempts(string username)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShopDB"].ConnectionString);
            conn.Open();
            string selectuser = "SELECT * FROM [User] WHERE Username=@username";
            SqlCommand selectusercmd = new SqlCommand(selectuser, conn);
            selectusercmd.Parameters.AddWithValue("@username", username);
            SqlDataReader userread = selectusercmd.ExecuteReader();
            userread.Read();
            if (userread["LoginAttempts"].ToString() == "")
            {
                userread.Close();
                string setattempt = "UPDATE [User] SET LoginAttempts=@attempt WHERE Username=@username";
                SqlCommand setattemptcmd = new SqlCommand(setattempt, conn);
                setattemptcmd.Parameters.AddWithValue("@username", username);
                setattemptcmd.Parameters.AddWithValue("@attempt", 1);
                setattemptcmd.ExecuteNonQuery();

            }

            else
            {
                userread.Close();
                string setattempt = "UPDATE [User] SET LoginAttempts = LoginAttempts + @attempt WHERE Username=@username";
                SqlCommand setattemptcmd = new SqlCommand(setattempt, conn);
                setattemptcmd.Parameters.AddWithValue("@username", username);
                setattemptcmd.Parameters.AddWithValue("@attempt", 1);
                setattemptcmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    }
}