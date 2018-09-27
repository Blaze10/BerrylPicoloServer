using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoProjectAPI.DTO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Utility.DataAccessUtility;

namespace DemoProjectAPI.Repository
{
    public class AllModuleRepository
    {
        //Encryption 
        public string encryption(string Password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(Password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
        }

       //User Login / Registration
       public LoginStatusDTO UserLogin(UserLoginDTO dto)
        {
            LoginStatusDTO ob = new LoginStatusDTO();
            DataSet ds = new DataSet();
            string EncPassword = encryption(dto.Password);
            try
            {
                string SP_Name = "UserMaster_Insert_Update";
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@UserID", dto.UserID);
                param[1] = new SqlParameter("@FirstName", dto.FirstName);
                param[2] = new SqlParameter("@LastName", dto.LastName);
                param[3] = new SqlParameter("@MobileNumber", dto.MobileNumber);
                param[4] = new SqlParameter("@Email", dto.Email);
                param[5] = new SqlParameter("@Password",EncPassword);
                param[6] = new SqlParameter("@StatementType", dto.StatementType);
                ds = DBConnection.ExecuteDataSetSPDB(SP_Name, param);
            }
            catch(Exception) { throw; }


            if (dto.StatementType == "Insert")
            {
                if(ds!=null && ds.Tables[0].Rows.Count > 0)
                {
                    int IsExists = Convert.ToInt32(ds.Tables[0].Rows[0]["IsExists"].ToString());
                    if(IsExists == 0)
                    {
                        ob.Response = "User Registration Successful!";
                        ob.MobileStatus = "1";
                        return ob;
                    }
                    else if(IsExists > 0)
                    {
                        ob.Response = "This Email or Mobile Number is already taken";
                        ob.MobileStatus = "0";
                        return ob;
                    }
                    else
                    {
                        ob.Response = "Opps some Error Occured";
                        ob.MobileStatus = "0";
                        return ob;
                    }
                }
                else
                {
                    ob.Response = "Opps some Error Occured";
                    ob.MobileStatus = "0";
                    return ob;
                }
            }

            else if(dto.StatementType == "Login")
            {
                if(ds!=null && ds.Tables[0].Rows.Count > 0)
                {
                    ob.UserID = ds.Tables[0].Rows[0]["UserID"].ToString();
                    ob.MobileStatus = "1";
                    ob.Response = "Login Successful";
                    return ob;
                }
                else
                {
                    ob.MobileStatus = "0";
                    ob.Response = "Invalid Email or Password";
                    return ob;
                }
            }

            else if(dto.StatementType == "Update")
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int IsUpdated = Convert.ToInt32(ds.Tables[0].Rows[0]["Updated"].ToString());
                    if (IsUpdated == 1)
                    {
                        ob.MobileStatus = "1";
                        ob.Response = "User data updation successful";
                        return ob;
                    }
                    else
                    {
                        ob.MobileStatus = "0";
                        ob.Response = "Oops some error occured";
                        return ob;
                    }
                }
                else
                {
                    ob.MobileStatus = "0";
                    ob.Response = "Oops some error occured";
                    return ob;
                }
            }

            else
            {
                ob.MobileStatus = "0";
                ob.Response = "Oops some error occured";
                return ob;
            }
        }

        //Video Master Insert
        public VideoMasterStatus VideoMaster(VideoMasterDTO dto)
        {
            VideoMasterStatus ob = new VideoMasterStatus();
            VideoMasterDTO obj = new VideoMasterDTO();
            List<VideoMasterDTO> list = new List<VideoMasterDTO>();
            DataSet Ds = new DataSet();
            int RowsAffected = 0;
        

            String Sp_name = "VideoMaster_Insert";
            SqlParameter[] param = new SqlParameter[4];
            param[0] = new SqlParameter("@VideoID",dto.VideoID);
            param[1] = new SqlParameter("@VideoString",dto.VideoString);
            param[2] = new SqlParameter("@UserID",dto.UserID);
            param[3] = new SqlParameter("@StatementType",dto.StatementType);

            if(dto.StatementType == "Insert" || dto.StatementType == "Delete")
            {
                try
                {
                    RowsAffected = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionString(), CommandType.StoredProcedure, Sp_name, param);
                }
                catch (Exception) { throw; }

                if(RowsAffected > 0)
                {
                     ob.MobileStatus = "1";
                     return ob;
                }
                else
                {
                    ob.MobileStatus = "0";
                    return ob;
                }
            }
            else
            {
                if(dto.StatementType == "ShowVideos")
                {
                    try
                    {
                        Ds = SqlHelper.ExecuteDataset(SqlHelper.ConnectionString(), CommandType.StoredProcedure, Sp_name, param);
                    }
                    catch (Exception) { throw; }
                    if(Ds!=null && Ds.Tables[0].Rows.Count > 0)
                    {
                        for(int i=0;i<Ds.Tables[0].Rows.Count; i++)
                        {
                            obj = new VideoMasterDTO(Ds, i);
                            list.Add(obj);
                        }
                        ob.list = list;
                        ob.MobileStatus = "1";
                        return ob;
                    }
                    else
                    {
                        ob.MobileStatus = "0";
                        return ob;
                    }
                }
                else
                {
                    ob.MobileStatus = "0";
                    return ob;
                }
            }

           


        }

        //Share Video Master
        public ShareVideoInsertStatus ShareVideoMaster(ShareVideoInsetDTO dto)
        {
            ShareVideoInsertStatus obj = new ShareVideoInsertStatus();
            ShareVideoDTO ob = new ShareVideoDTO();
            List<ShareVideoDTO> list = new List<ShareVideoDTO>();
            int RowsAffected = 0;
            DataSet ds = new DataSet();

            if(dto.StatementType == "Insert" || dto.StatementType == "Delete")
            {
                foreach (var item in dto.dtoItem)
                {
                    string Sp_Name = "ShareVideo_Insert";
                    SqlParameter[] param = new SqlParameter[5];
                    try
                    {
                        param[0] = new SqlParameter("@ID",item.ID);
                        param[1] = new SqlParameter("@VideoID",item.VideoID);
                        param[2] = new SqlParameter("@UserID",item.UserID);
                        param[3] = new SqlParameter("@SharedTo",item.ShareTo);
                        param[4] = new SqlParameter("@StatementType",dto.StatementType);
                        RowsAffected += DBConnection.ExecuteNonQuery(Sp_Name, param);
                    }
                    catch(Exception) { throw; }
                }
                if(RowsAffected > 0)
                {
                    obj.MobileStatus = "1";
                    return obj;
                }
                else
                {
                    obj.MobileStatus = "0";
                    return obj;
                }
            }
            else
            {
                if(dto.StatementType == "ShowVideos")
                {
                    string Sp_Name = "ShareVideo_Insert";
                    SqlParameter[] param = new SqlParameter[5];
                    try
                    {
                        param[0] = new SqlParameter("@ID", dto.ID);
                        param[1] = new SqlParameter("@VideoID", dto.VideoID);
                        param[2] = new SqlParameter("@UserID", dto.UserID);
                        param[3] = new SqlParameter("@SharedTo", dto.ShareTo);
                        param[4] = new SqlParameter("@StatementType", dto.StatementType);
                        ds = DBConnection.ExecuteDataSetSPDB(Sp_Name, param);
                    }
                    catch (Exception) { throw; }
                    if(ds!=null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ob = new ShareVideoDTO(ds, i);
                            list.Add(ob);
                        }
                        obj.list = list;
                        obj.MobileStatus = "1";
                        return obj;
                    }
                    else
                    {
                        obj.MobileStatus = "0";
                        return obj;
                    }
                }
                else
                {
                    obj.MobileStatus = "0";
                    return obj;
                }
            }
           


        }

    }
}