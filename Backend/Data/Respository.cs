using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using retplann.Model;
using Microsoft.Extensions.Logging;


namespace retplann.Data
{
    public class Repository
    {
        private readonly string _connectionString;
        private readonly ILogger<Repository> _logger;
        public Repository(string connectionString, ILogger<Repository> logger)
        {
            _connectionString=connectionString;
            _logger=logger;
        }
        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public Profile ValidateUser(string email, string password)
        {
            Profile pf = null;
            _logger.LogInformation("User Trying to Login");
            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                return pf;
            }   
            try
            {
                // _logger.LogInformation("User Trying to Login");
                using(var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new MySqlCommand("ValidateUser", connection))
                    {
                        command.CommandType=CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_email", email);
                        command.Parameters.AddWithValue("@p_password", password);
                        using(var reader = command.ExecuteReader())
                        {
    
                            if(reader.Read())       //if user is found
                            {
                                _logger.LogInformation("User Logged In Successfully");
                                pf = new Profile{
                                    PId=Convert.ToInt32(reader["PId"]),
                                    FirstName=reader["FirstName"].ToString(),
                                    LastName=reader["LastName"].ToString(),
                                    Age=Convert.ToInt32(reader["Age"]),
                                    Gender=reader["Gender"].ToString(),
                                    Email=reader["Email"].ToString()
                                };
                                return pf;
                            }
                            else
                            {
                                _logger.LogError("User Log In Failed");
                            }
        
                        }
                    }
                    connection.Close();
                }
                return pf; //if null this will execute
                
            }
            catch(MySqlException ex)
            {
                _logger.LogError("MySQL Error: ", ex.Message);
                throw new Exception($"Error: Database error while validating user");
            }
            catch(Exception ex)
            {
                _logger.LogError("General Error: {Message}", ex.Message);
                throw;
            }
        }
        // public void StoreGoal(Goal_plan g_p)
        // {
        //     if (g_p == null)
        //     {
        //         _logger.LogWarning("Goal Plan is null.");
        //     }
        //     if ((g_p.PId == null || g_p.PId <= 0) || (g_p.CurrentAge == null || g_p.CurrentAge <= 0) || (g_p.RetirementAge == null || g_p.RetirementAge <= 0) || (g_p.CurrentSavings == null || g_p.CurrentSavings <= 0) || (g_p.TargetSavings == null || g_p.TargetSavings <= 0))
        //     {
        //         _logger.LogWarning("Invalid data: {GoalPlan}", g_p);
        //     }
        //     try
        //     {
        //         _logger.LogInformation("Trying to Insert Goal Plan");
        //         using(var connection = new MySqlConnection(_connectionString))
        //         {
        //             connection.Open();
        //             using(var command = new MySqlCommand("InsertGoalPlan", connection))
        //             {
        //                 command.CommandType=CommandType.StoredProcedure;
        //                 command.Parameters.AddWithValue("@p_PId", g_p.PId);
        //                 command.Parameters.AddWithValue("@p_currentAge", g_p.CurrentAge);
        //                 command.Parameters.AddWithValue("@p_retirementAge", g_p.RetirementAge);
        //                 command.Parameters.AddWithValue("@p_currentSavings", g_p.CurrentSavings);
        //                 command.Parameters.AddWithValue("@p_targetSavings", g_p.TargetSavings);
        //                 command.ExecuteNonQuery();
        //                 _logger.LogInformation("Goal Plan Inserted Successfully");
        //             }
        //             connection.Close();
        //         }
        //     }
        //     catch(MySqlException ex)
        //     {
        //         _logger.LogError("MySQL Error: ", ex.Message);
        //         throw new Exception($"Error: Database error while inserting goal plan");
        //     }
        //     catch(Exception ex)
        //     {
        //         _logger.LogError("General Error: {Message}", ex.Message);
        //         throw;
        //     }
        // }

        // public void Calculate(int pid)
        // {
        //     if (pid <= 0 || pid==null)
        //     {
        //         _logger.LogError("Invalid pid provided: {Pid}", pid);
        //     }
        //     try
        //     {
        //         using(var connection = new MySqlConnection(_connectionString))
        //         {
        //             connection.Open();
        //             using(var command = new MySqlCommand("CalculateMonthlyContribution", connection))
        //             {
        //                 command.CommandType=CommandType.StoredProcedure;
        //                 command.Parameters.AddWithValue("@p_pid",pid);
        //                 command.ExecuteNonQuery();
        //                 _logger.LogInformation("Monthly Contribution calulated successfully");
        //             }
        //             connection.Close();
        //         }
        //     }
        //     catch(MySqlException ex)
        //     {
        //         _logger.LogError("MySQL Error: ", ex.Message);
        //     }
            
            
        // }

        public Goal_plan StoreGoal(Goal_plan g_p)
        {
            if (g_p == null)
            {
                _logger.LogWarning("Goal Plan is null.");
                throw new ArgumentException("Goal Plan cannot be null.");
            }
            if ((g_p.PId == null || g_p.PId <= 0) || (g_p.CurrentAge == null || g_p.CurrentAge <= 0) || (g_p.RetirementAge == null || g_p.RetirementAge <= 0) || (g_p.CurrentSavings == null || g_p.CurrentSavings < 0) || (g_p.TargetSavings == null || g_p.TargetSavings <= 0))
            {
                _logger.LogWarning("Invalid data: {GoalPlan}", g_p);
                throw new ArgumentException("Invalid data: Ensure all values are valid and CurrentAge is less than RetirementAge.");
            }

            try
            {
                _logger.LogInformation("Trying to Insert Goal Plan and Calculate Monthly Contribution");

                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertGoalPlan", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_PId", g_p.PId);
                        command.Parameters.AddWithValue("@p_currentAge", g_p.CurrentAge);
                        command.Parameters.AddWithValue("@p_retirementAge", g_p.RetirementAge);
                        command.Parameters.AddWithValue("@p_currentSavings", g_p.CurrentSavings);
                        command.Parameters.AddWithValue("@p_targetSavings", g_p.TargetSavings);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                g_p.GId=reader.GetInt32("GId");
                                g_p.PId=reader.GetInt32("PId");
                                g_p.CurrentAge = reader.GetInt32("Current_Age");
                                g_p.RetirementAge = reader.GetInt32("Retirement_Age");
                                g_p.CurrentSavings = reader.GetDecimal("Current_Savings");
                                g_p.TargetSavings = reader.GetDecimal("Target_Savings");
                                g_p.MonthlyContribution = reader.GetDecimal("Monthly_Contribution");
                            }
                        }
                    }
                    connection.Close();
                }

                _logger.LogInformation("Goal Plan Inserted and Retrieved Successfully");
                return g_p; 
            }
            catch (MySqlException ex)
            {
                _logger.LogError("MySQL Error: {Message}", ex.Message);
                throw new Exception("Error: Database error while inserting goal plan and retrieving goal data.");
            }
            catch (Exception ex)
            {
                _logger.LogError("General Error: {Message}", ex.Message);
                throw;
            }
        }


        public Goal_plan GetUserGoalPlan(int pid)
        {
            Goal_plan goalPlan=null;
            if (pid <= 0 || pid==null)
            {
                _logger.LogWarning("Invalid pid provided: {Pid}", pid);
                return goalPlan;
            }
            try
            {
                _logger.LogInformation("Trying to aquire Goal Plan");
                using(var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new MySqlCommand("SELECT * FROM GoalPlan WHERE PId=@pid", connection))
                    {
                        command.Parameters.AddWithValue("@pid", pid);
                        using(var reader=command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                goalPlan = new Goal_plan
                                {
                                    GId=reader.GetInt32("GId"),
                                    PId=reader.GetInt32("PId"),
                                    CurrentAge=reader.GetInt32("Current_Age"),
                                    RetirementAge=reader.GetInt32("Retirement_Age"),
                                    CurrentSavings=reader.GetDecimal("Current_Savings"),
                                    TargetSavings=reader.GetDecimal("Target_Savings"),
                                    MonthlyContribution=reader.GetDecimal("Monthly_Contribution")
                                };
                                _logger.LogInformation("Goal Plan Acquired Successfully for User {PId}", pid);
                            }
                            else
                            {
                                _logger.LogError("Check your pid");
                            }
                        }
                    }
                }
            }
            
            catch(MySqlException ex)
            {
                _logger.LogError("MySQL Error while getting the goal details: ", ex.Message);
                throw new Exception($"Error: Database error while acquiring the goal plan");
            }
            catch(Exception ex)
            {
                _logger.LogError("General Error: {Message}", ex.Message);
                throw;
            }
            return goalPlan;
        }
        public void InsertFinancialYear(FinancialYear fy)
        {
            if (fy == null)
            {
                _logger.LogError("Financial Year is null.");
                return;
            }
            if ((fy.GId == null || fy.GId <= 0) || (fy.Year == null || fy.Year <= 0) || (fy.MonthNumber == null || fy.MonthNumber <= 0) || (fy.MonthlyInvestment == null || fy.MonthlyInvestment <= 0))
            {
                _logger.LogError("Invalid data: {FinancialYear}", fy);
                return;
            }
            try
            {
                _logger.LogInformation("Trying to insert Investment Activity");
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand("InsertFinancialYear", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_GId", fy.GId);
                        command.Parameters.AddWithValue("@p_Year", fy.Year);
                        command.Parameters.AddWithValue("@p_Month", fy.MonthNumber);
                        command.Parameters.AddWithValue("@p_MonthlyInvestment", fy.MonthlyInvestment);
                        command.ExecuteNonQuery();
                        _logger.LogInformation("Successfully made an entry");
                    }
                    connection.Close();
                }
            }
             catch (MySqlException ex)
            {
                // Check if the error message matches the duplicate entry signal
                if (ex.Message.Contains("Duplicate entry"))
                {
                    _logger.LogError("Already invested for the mentioned month");
                }
                else
                {
                    _logger.LogError("Database Error: {Message}", ex.Message);
                }
            }
            
        }
        public Progress getSavings(int gid)
        {
            Progress progress=null;
            if (gid <= 0 || gid==null)
            {
                _logger.LogWarning("Invalid gid provided: {Gid}", gid);
                return progress;
            }
            try
            {
                _logger.LogInformation("Trying to aquire TotalSavings");
                using(var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    using(var command = new MySqlCommand("SELECT Savings FROM user_progress WHERE GId=@gid", connection))
                    {
                        command.Parameters.AddWithValue("@gid", gid);
                        using(var reader=command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                progress = new Progress
                                {
                                    Total_Savings=reader.GetDecimal("Savings")
                                };
                                _logger.LogInformation("Total_Savings Acquired Successfully for Goal {GId}", gid);
                            }
                            else
                            {
                                _logger.LogError("Check your gid");
                            }
                        }
                    }
                }
            }
            
            catch(MySqlException ex)
            {
                _logger.LogError("MySQL Error while getting the total savings: ", ex.Message);
                throw new Exception($"Error: Database error while acquiring the total savings");
            }
            catch(Exception ex)
            {
                _logger.LogError("General Error: {Message}", ex.Message);
                throw;
            }
            return progress;
        }


    }
}
