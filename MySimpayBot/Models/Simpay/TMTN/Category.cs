using Shared.WebService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Models.TMTN
{
    public class Category
    {
        public List<CategoryData> data { get; set; }


        public Category(int simpayId, bool readFromData = false)
        {
            data = new List<CategoryData>();
            if (readFromData)
            {
                getInfo(simpayId);
            }
        }
        public void getInfo(int simpayId)
        {

            do
            {
                var list = new List<CategoryData>();
                var result = DataBase.GetTMTNSimTypeCategory(simpayId);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }
                if (result.DataSet.Tables[0].Rows.Count <= 0)
                {
                    break;
                }
                var DS = Converter.DBNull(result.DataSet);
                data.Clear();
                foreach (DataRow record in DS.Tables[0].Rows)
                {
                    list.Add(new CategoryData()
                    {
                        categoryId = (int)record["categoryId"],
                        categoryName = (string)record["categoryName"],
                        categoryShowName = (string)record["categoryShowName"],
                        categoryThumbnail = (string)record["categoryThumbnail"]
                    });
                }
                data.AddRange(list);

            } while (false);

        }
        public void setInfo(int simTypeId)
        {
            var result = DataBase.SetTMTNSimTypeCategory(simTypeId, data);
            if (result.ReturnCode != 1 || result.SPCode != 1)
            {
                Log.Fatal(result.Text, DateTime.Now.Millisecond);
            }

        }
        public string GetCategoryShowName(int simTypeId)
        {
            var ans = "";
            var selectedResult = data.Find(delegate (CategoryData sd)
            {
                return (sd.categoryId == simTypeId);
            });
            if (selectedResult != null)
                ans = selectedResult.categoryShowName;

            return ans;
        }


    }
    public class CategoryData
    {
        public int categoryId { get; set; }

        public string categoryName { get; set; }

        public string categoryShowName { get; set; }

        public string categoryThumbnail { get; set; }


    }
}