using System;
using System.Collections.Generic;
using Shared.WebService;

namespace Models
{
    public class Menu
    {
        public long chatId { get; set; }
        public bool hasParent { get; set; }

        public List<MenuItem> menuItem { get; set; }
        public Menu(long currentChatId) : this(currentChatId, 999)
        {

        }
        public Menu(long currentChatId, int selectedMenuId = 0)
        {
            chatId = currentChatId;
            do
            {
                var result = DataBase.getMenu(chatId, selectedMenuId);
                if (result.ReturnCode != 1 || result.SPCode != 1)
                {
                    Log.Fatal(result.Text, DateTime.Now.Millisecond);
                    break;
                }

                if (result.DataSet.Tables[0].Rows.Count > 0)
                {

                    var ds = Converter.DBNull(result.DataSet);
                    var DR = ds.Tables[0].Rows;
                    menuItem = new List<MenuItem>();
                    for (var row = 0; row < DR.Count; row++)
                    {
                        menuItem.Add(new MenuItem
                        {
                            menuId = (int)DR[row]["menuId"],
                            menuCaption = (string)DR[row]["menuCaption"],
                            menuKey = (string)DR[row]["menuKey"],
                            newLine = (bool)DR[row]["newLine"],
                            parentId = (int)DR[row]["parentId"]
                        });
                    }

                    hasParent = (bool)ds.Tables[1].Rows[0]["hasParent"];
                }
            } while (false);

        }


        public class MenuItem
        {
            public int menuId { get; set; }
            public string menuCaption { get; set; }
            public string menuKey { get; set; }
            public int parentId { get; set; }
            public bool newLine { get; set; }
        }
    }


}