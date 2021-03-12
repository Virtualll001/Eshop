using Eshop.Classes;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderFlashMessages(this IHtmlHelper helper)
        {
            // pod klíčem "Messages" najdeme pole se všemi zprávami ve formátu json, které rovnou pomocí extension metody deserializujeme
            // pozn.: pokud jsme žádné zprávy neuložili, metoda vrátí prázdnou instanci Listu - což nám ale vůbec nevadí
            var messageList = helper.ViewContext.TempData
                .DeserializeToObject<List<FlashMessage>>("Messages");

            var html = new HtmlContentBuilder();

            // procházíme všechny zprávy a vytvoříme HTML
            foreach (var msg in messageList)
            {
                var container = new TagBuilder("div");
                container.AddCssClass($"alert alert-{ msg.Type.ToString().ToLower() }"); //přidáme CSS z Bootstrap
                container.InnerHtml.SetContent(msg.Message);

                html.AppendHtml(container);
            }
            return html;
        }
    }
}
