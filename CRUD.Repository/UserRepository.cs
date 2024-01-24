using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using CRUD.Model;
using CRUD.Common;
using CRUD.Contract;

namespace CRUD.Repository
{
    public class UserRepository : IUserContract
    {
        public readonly IDBContext _dbContext;
        public readonly IConfiguration _configuration;

        public UserRepository(IDBContext dBContext, IConfiguration configuration)
        {
            _dbContext = dBContext;
            _configuration = configuration;
        }

        public async Task<LoggedInUser> AuthenticateUser(LoginData data)
        {
            LoggedInUser userResponse = new LoggedInUser();
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@USERNAME",data.USER_NAME),
                    new SqlParameter("@PASSWORD",data.PASSWORD)
                };

                DataSet ds = await _dbContext.GetDataSetAsync(DBConstant.USP_USER_LOGIN, parameters);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ResultResponse res = new ResultResponse();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            res.FLAG = Convert.ToBoolean(ds.Tables[0].Rows[0]["FLAG"]);
                            res.MESSAGE = Convert.ToString(ds.Tables[0].Rows[0]["MSG"]);
                        }

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            if (res.FLAG)
                            {
                                userResponse.USER_ID = Convert.ToInt32(ds.Tables[1].Rows[0]["USER_ID"]);
                                userResponse.FULL_NAME = Convert.ToString(ds.Tables[1].Rows[0]["FULL_NAME"]);
                                userResponse.MOBILE_NO = Convert.ToString(ds.Tables[1].Rows[0]["MOBILE_NO"]);
                                userResponse.EMAIL = Convert.ToString(ds.Tables[1].Rows[0]["EMAIL"]);
                                userResponse.RESULT = res;
                            }
                            else
                            {
                                userResponse.RESULT = res;
                            }

                            if (res.FLAG)
                            {
                                userResponse.TOKEN = Common.Common.GenerateToken(_configuration, Convert.ToString(userResponse.USER_ID), userResponse.EMAIL, "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> UserRepository -> GetUserDetailsByID", "Error Occured -> " + ex.Message);
                userResponse = new LoggedInUser();
            }

            return userResponse;
        }

        public async Task<ResultResponse> DeleteUser(int userID)
        {
            ResultResponse res = new ResultResponse();
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@USER_ID", userID)
                };

                DataSet ds = await _dbContext.GetDataSetAsync(DBConstant.USP_DELETE_USER, parameters);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        res.FLAG = Convert.ToBoolean(ds.Tables[0].Rows[0]["FLAG"]);
                        res.MESSAGE = Convert.ToString(ds.Tables[0].Rows[0]["MSG"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> UserRepository -> SaveUserDetails", "Error Occured -> " + ex.Message);
                res = new ResultResponse
                {
                    FLAG = false,
                    MESSAGE = ex.Message
                };
            }

            return res;
        }

        public async Task<Dictionary<string, object>> GetUserDetailsByID(int userID)
        {
            User res = new User();
            Dictionary<string, object> userDetails = new Dictionary<string, object>();
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@USER_ID", userID)
                };

                DataSet ds = await _dbContext.GetDataSetAsync(DBConstant.USP_GET_USER_DETAILS_BY_ID, parameters);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        res.USER_ID = Convert.ToInt32(ds.Tables[0].Rows[0]["USER_ID"]);
                        res.FULL_NAME = Convert.ToString(ds.Tables[0].Rows[0]["FULL_NAME"]);
                        res.MOBILE_NO = Convert.ToString(ds.Tables[0].Rows[0]["MOBILE_NO"]);
                        res.EMAIL = Convert.ToString(ds.Tables[0].Rows[0]["EMAIL"]);
                        res.USER_NAME = Convert.ToString(ds.Tables[0].Rows[0]["USER_NAME"]);
                        res.PASSWORD = Convert.ToString(ds.Tables[0].Rows[0]["PASSWORD"]);

                        userDetails["USER_DETAILS"] = res;
                        userDetails["RESULT"] = new ResultResponse
                        {
                            FLAG = true,
                            MESSAGE = "Successful"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> UserRepository -> GetUserDetailsByID", "Error Occured -> " + ex.Message);
                userDetails["USER_DETAILS"] = null;
                userDetails["RESULT"] = new ResultResponse
                {
                    FLAG = false,
                    MESSAGE = ex.Message
                };
            }

            return userDetails;
        }

        public async Task<UserList> GetUserList()
        {
            UserList res = new UserList();
            try
            {
                SqlParameter[] parameters = {};

                DataSet ds = await _dbContext.GetDataSetAsync(DBConstant.USP_GET_USER_LIST, parameters);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        res.USER_LIST = Common.Common.ConvertDTintoList<User>(ds.Tables[0]);
                        res.RESULT = new ResultResponse
                        {
                            FLAG = true,
                            MESSAGE = "Successful"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> UserRepository -> GetUserList", "Error Occured -> " + ex.Message);
                res.USER_LIST = null;
                res.RESULT = new ResultResponse
                {
                    FLAG = false,
                    MESSAGE = ex.Message
                };
            }

            return res;
        }

        public async Task<ResultResponse> SaveUserDetails(User data)
        {
            ResultResponse res = new ResultResponse();
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@USER_ID",data.USER_ID),
                    new SqlParameter("@FULL_NAME",data.FULL_NAME),
                    new SqlParameter("@MOBILE_NO",data.MOBILE_NO),
                    new SqlParameter("@EMAIL",data.EMAIL),
                    new SqlParameter("@USER_NAME",data.USER_NAME),
                    new SqlParameter("@PASSWORD",data.PASSWORD)
                };

                DataSet ds = await _dbContext.GetDataSetAsync(DBConstant.USP_SAVE_USER_DETAILS, parameters);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        res.FLAG = Convert.ToBoolean(ds.Tables[0].Rows[0]["FLAG"]);
                        res.MESSAGE = Convert.ToString(ds.Tables[0].Rows[0]["MSG"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.WriteToLog("Repository -> UserRepository -> SaveUserDetails", "Error Occured -> " + ex.Message);
                res = new ResultResponse
                {
                    FLAG = false,
                    MESSAGE = ex.Message
                };
            }

            return res;
        }
    }
}
