using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoProjectAPI.DTO
{
   public class UserLoginDTO
    {
        public UserLoginDTO() { }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string StatementType { get; set; }
    }
    public class LoginStatusDTO
    {
        public string MobileStatus { get; set; }
        public string Response { get; set; }
        public string UserID { get; set; }
    }

    //Video Insert
    public class VideoMasterDTO
    {
        public VideoMasterDTO() { }
        public VideoMasterDTO(DataSet ds, int i)
        {
            if(ds!=null && ds.Tables[0].Rows[i]["VideoID"].ToString() != "")
            {
                VideoID = ds.Tables[0].Rows[i]["VideoID"].ToString();
            }
            else
            {
                VideoID = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["VideoString"].ToString() != "")
            {
                VideoString = ds.Tables[0].Rows[i]["VideoString"].ToString();
            }
            else
            {
                VideoString = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["UserID"].ToString() != "")
            {
                UserID = ds.Tables[0].Rows[i]["UserID"].ToString();
            }
            else
            {
                UserID = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["WhenCreated"].ToString() != "")
            {
                WhenUploaded = ds.Tables[0].Rows[i]["WhenCreated"].ToString();
            }
            else
            {
                WhenUploaded = "";
            }
        }
        public string VideoID { get; set; }
        public string VideoString { get; set; }
        public string UserID { get; set; }
        public string WhenUploaded { get; set; }
        public string StatementType { get; set; }
    }
    public class VideoMasterStatus
    {
        public string MobileStatus { get; set; }
        public List<VideoMasterDTO> list { get; set; }
    }

    //Share Video
    public class ShareVideoDTO
    {
        public ShareVideoDTO() { }
        public ShareVideoDTO(DataSet ds, int i)
        {
            if (ds != null && ds.Tables[0].Rows[i]["ID"].ToString() != "")
            {
                ID = ds.Tables[0].Rows[i]["ID"].ToString();
            }
            else
            {
                ID = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["VideoString"].ToString() != "")
            {
                VideoID = ds.Tables[0].Rows[i]["VideoString"].ToString();
            }
            else
            {
                VideoID = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["FirstName"].ToString() != "" && ds.Tables[0].Rows[i]["LastName"].ToString() != "")
            {
                UserID = ds.Tables[0].Rows[i]["FirstName"].ToString() + " " + ds.Tables[0].Rows[i]["LastName"].ToString();
            }
            else
            {
                UserID = "";
            }
            if (ds != null && ds.Tables[0].Rows[i]["WhenCreated"].ToString() != "")
            {
                WhenShared = ds.Tables[0].Rows[i]["WhenCreated"].ToString();
            }
            else
            {
                WhenShared = "";
            }
        }
        public string ID { get; set; }
        public string VideoID { get; set; }
        public string UserID { get; set; }
        public string ShareTo { get; set; }
        public string WhenShared { get; set; }
    }

    public class ShareVideoInsetDTO
    {

        public ShareVideoInsetDTO() { }
        public List<ShareVideoDTO> dtoItem { get; set; }
        public string StatementType { get; set; }
        public string ID { get; set; }
        public string VideoID { get; set; }
        public string UserID { get; set; }
        public string ShareTo { get; set; }
        public string WhenShared { get; set; }
    }

    public class ShareVideoInsertStatus
    {
        public string MobileStatus { get; set; }
        public List<ShareVideoDTO> list { get; set; }
    }


}