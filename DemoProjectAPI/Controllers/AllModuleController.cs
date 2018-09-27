using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DemoProjectAPI.DTO;
using DemoProjectAPI.Repository;

namespace DemoProjectAPI.Controllers
{
    public class AllModuleController : ApiController
    {
    }

    public class UserLoginController: ApiController
    {
        [HttpPost]
        public LoginStatusDTO UserLogin(UserLoginDTO dto)
        {
            return new AllModuleRepository().UserLogin(dto);
        }
    }

    public class VideoMasterController: ApiController
    {
        [HttpPost]
        public VideoMasterStatus VideoMaster(VideoMasterDTO dto)
        {
            return new AllModuleRepository().VideoMaster(dto);
        }
    }

    public class ShareVideoMasterController: ApiController
    {
        [HttpPost]
        public ShareVideoInsertStatus ShareVideoMaster(ShareVideoInsetDTO shareVideoInsetDTO)
        {
            return new AllModuleRepository().ShareVideoMaster(shareVideoInsetDTO);
        }
    }

}
