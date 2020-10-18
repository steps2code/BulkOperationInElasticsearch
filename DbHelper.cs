using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace BulkOperationDemo
{
    public class DbHelper
    {
        const string DB_CONNECTION_STRING = "Data Source=HP;Initial Catalog=TestDB;User Id=sa;Password=Yash#123@;";
        public static List<Product> GetProductCatalog()
        {
            string sqlCommandText = "SELECT * FROM Products";
            List<Product> productList = new List<Product>();
            using (SqlConnection connection = new SqlConnection(DB_CONNECTION_STRING))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sqlCommandText, connection))
                {
                    command.CommandTimeout = 1000;
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                string csvKeyWordString = Convert.ToString(dataReader["KeyWords"]);
                                Product product = new Product
                                {
                                    id = Convert.ToString(dataReader["Id"]),
                                    name = Convert.ToString(dataReader["Name"]),
                                    category = Convert.ToString(dataReader["Category"]),
                                    description = Convert.ToString(dataReader["Description"]),
                                    isPopularProduct = Convert.ToInt16(dataReader["IsPopularProduct"]),
                                    price = Convert.ToInt16(dataReader["Price"]),
                                    keyWords = GetKeyWordList(csvKeyWordString)
                                };

                                productList.Add(product);
                            }
                        }
                    }
                }
                connection.Close();
            }

            return productList;
        }

        private static List<KeyWord> GetKeyWordList(string csvKeyWordString)
        {
            List<KeyWord> keyWordObjList = new List<KeyWord>();
            if (!string.IsNullOrEmpty(csvKeyWordString))
            {
                List<string> keyWordStringArray = csvKeyWordString.Split(',').ToList();
                foreach (string keywordString in keyWordStringArray)
                {
                    KeyWord KeyWord = new KeyWord()
                    {
                        input = new List<string>() { keywordString }
                    };
                    keyWordObjList.Add(KeyWord);
                }
            }
            return keyWordObjList;
        }
    }
}
