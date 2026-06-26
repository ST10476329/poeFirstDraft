using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace poeFirstDraft
{
    internal class DatabaseHelper
    {
        public static readonly string connectionString =
              @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=chatbotcyberdb;Integrated Security=True";
        // test the connection when the app starts
        public static bool TestConnection()
        {// start of TestConnection
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Database connection failed: " + e.Message);
                return false;
            }
        }// end of TestConnection

        // adds a new task to the database for this user
        public static void AddTask(string username, string title, string description, string reminder)
        {// start of AddTask
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO tasks (username, title, description, reminder) " +
                                   "VALUES (@username, @title, @description, @reminder)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@description", description);
                    cmd.Parameters.AddWithValue("@reminder", reminder);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error adding task: " + e.Message);
            }
        }// end of AddTask

        // gets all tasks for this user from the database
        public static List<string[]> GetTasks(string username)
        {// start of GetTasks
            List<string[]> tasks = new List<string[]>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT id, title, description, reminder, is_completed " +
                                   "FROM tasks WHERE username = @username";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        // store each task as an array of strings
                        tasks.Add(new string[]
                        {
                            reader["id"].ToString(),
                            reader["title"].ToString(),
                            reader["description"].ToString(),
                            reader["reminder"].ToString(),
                            reader["is_completed"].ToString()
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error getting tasks: " + e.Message);
            }

            return tasks;

        }// end of GetTasks

        // marks a task as completed in the database
        public static void CompleteTask(int taskId)
        {// start of CompleteTask
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Changed TRUE to 1 because MS SQL Server uses 1 for BIT (boolean) values
                    string query = "UPDATE tasks SET is_completed = 1 WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error completing task: " + e.Message);
            }
        }// end of CompleteTask

        // deletes a task from the database permanently
        public static void DeleteTask(int taskId)
        {// start of DeleteTask
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM tasks WHERE id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", taskId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error deleting task: " + e.Message);
            }
        }// end of DeleteTask
    }
}