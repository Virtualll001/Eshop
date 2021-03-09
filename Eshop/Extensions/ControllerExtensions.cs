using Eshop.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddFlashMessage(this Controller controller, FlashMessage message)
        {
            // stejně jako u rozšíření pro HtmlHelper, také zde můžeme pracovat i s prázdným seznamem
            var list = controller.TempData.DeserializeToObject<List<FlashMessage>>("Messages");

            list.Add(message);

            // uložíme rozšířený seznam zpráv zpátky do formátu json a kolekce TempData
            controller.TempData.SerializeObject(list, "Messages");
        }

        public static void AddFlashMessage(this Controller controller, string message, FlashMessageType messageType)
        {
            controller.AddFlashMessage(new FlashMessage(message, messageType));
        }
    }
}
