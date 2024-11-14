using System.Linq;
using System.Web.Mvc;
using DAWeb.Models;

namespace DAWeb.Filters
{
    public class CartItemCountFilter : ActionFilterAttribute
    {
        // Phương thức này sẽ được gọi trước khi một action trong controller được thực thi
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Lấy Session hiện tại
            var session = filterContext.HttpContext.Session;
            if (session != null)
            {
                // Kiểm tra xem có giỏ hàng trong session hay không
                var cart = session["Cart"] as GioHang;

                // Nếu giỏ hàng có tồn tại, tính tổng số lượng sản phẩm
                if (cart != null)
                {
                    filterContext.Controller.ViewBag.CartItemCount = cart.Items.Sum(i => i.SoLuong);
                }
                else
                {
                    // Nếu không có giỏ hàng, số lượng sản phẩm bằng 0
                    filterContext.Controller.ViewBag.CartItemCount = 0;
                }
            }

            // Gọi phương thức cơ bản của ActionFilterAttribute
            base.OnActionExecuting(filterContext);
        }
    }
}
