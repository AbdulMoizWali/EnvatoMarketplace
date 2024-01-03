using EnvatoMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace EnvatoMarketplace.Controllers
{
    enum CartStatus
    {
        CREATED,
        ACTIVE,
        PENDING,
        DELIVERED
    }

    enum PaymentType
    {
        Cash = 1,
        Card = 2,
    }

    public class CartController : Controller
    {
        EnvatoDBEntities db = new EnvatoDBEntities();
        // GET: Cart
        public ActionResult Index()
        {
            Session["username"] = "o";
            Session["uid"] = "55";
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var username = Session["username"].ToString();
            var user = db.Users.First(usr => usr.username == username);
            try
            {
                var cart = user.Carts.LastOrDefault(crt => crt.cartStatus == CartStatus.ACTIVE.ToString());
                var cartItems = cart?.CartItems ;
                /*List<Product> productList = new List<Product>();

                foreach (var cartItem in cartItems)
                {
                    var product = new Product();
                    product.uid = cart.uid;
                    cartItem.pName;
                    cartItem.pid;
                    cartItem.pShortDesc;
                    cartItem.pBuyQty;
                    cartItem
                }*/

                return View(cartItems?.ToList() ?? new List<CartItem>());
            }
            catch (Exception)
            {
                return RedirectToAction("NoProductInCart");
            }
        }

        public ActionResult NoProductInCart()
        {
            return View();
        }

        public ActionResult AddToCart(int? id, string BuyQty)
        {
            if (Session["username"] == null) {
                return RedirectToAction("Login", "Auth");
            }
            int pid = (int)id;
            int uid = int.Parse(Session["uid"].ToString());
            int buyQty = int.Parse(BuyQty);

            Product product = db.Products.FirstOrDefault(prdct => prdct.pid == pid);
            User user = db.Users.FirstOrDefault(usr => usr.uid == uid);

            if (product.availableQty != null)
            {
                if (product.availableQty < buyQty)
                {
                    return RedirectToAction("Index");
                }
                product.availableQty -= buyQty;
            }


            if (user.Carts.Count > 0)
            {
                /*try
                {*/
                    Cart cart = user.Carts.LastOrDefault();
                    if (cart.cartStatus == CartStatus.CREATED.ToString() || cart.cartStatus == CartStatus.ACTIVE.ToString())
                    {
                        if(cart.cartStatus == CartStatus.CREATED.ToString())
                        {
                            Debug.WriteLine("CartStatus.CREATED");
                            cart.cartStatus = CartStatus.ACTIVE.ToString();
                        }
                        else
                        {
                            Debug.WriteLine("CartStatus.ACTIVE");
                        }

                        


                        if (cart.CartItems.Any(crtItem => crtItem.Product.pid == pid))
                        {
                            var existingCartItem = cart.CartItems.Where(crtItem => crtItem.Product.pid == pid).FirstOrDefault();
                            existingCartItem.pBuyQty = buyQty;
                            db.CartItems.AddOrUpdate(existingCartItem);
                        }
                        else
                        {
                            CartItem newCartItem = new CartItem();
                            newCartItem.cid = cart.cid;
                            newCartItem.pid = pid;
                            newCartItem.pName = product.name;
                            newCartItem.pShortDesc = product.shortDesc;
                            newCartItem.pAmount = product.amount;
                            newCartItem.pBuyQty = buyQty;
                            db.CartItems.Add(newCartItem);
                        }

                        int totalAmount = 0;
                        foreach (var cartItem in cart.CartItems)
                        {
                            totalAmount += (int)(cartItem.pAmount * cartItem.pBuyQty);
                        }
                        cart.totalAmount = totalAmount;

                        db.Carts.AddOrUpdate(cart);
                        db.Products.AddOrUpdate(product);
                        db.SaveChanges();
                    }
                    /*else if (cart.cartStatus == CartStatus.DELIVERED.ToString())
                    {
                        Debug.WriteLine("CartStatus.DELIVERED");
                        Cart newCartOpen = CreateNewCart(uid);

                        db.Carts.Add(newCartOpen);
                        if (db.SaveChanges() > 0)
                        {
                            EnvatoDBEntities db1 = new EnvatoDBEntities();
                            Cart newCartCreated = db1.Carts.LastOrDefault(crt => crt.uid == uid);

                            CartItem newCartItem = new CartItem();
                            newCartItem.cid = newCartCreated.cid;
                            newCartItem.pid = pid;
                            newCartItem.pName = product.name;
                            newCartItem.pShortDesc = product.shortDesc;
                            newCartItem.pAmount = product.amount;
                            newCartItem.pBuyQty = buyQty;

                            db1.CartItems.Add(newCartItem);
                            db1.SaveChanges();
                        }
                    }*/
                /*}
                catch (Exception ex)
                {
                    Debug.WriteLine("Cathed exception: " + ex.Message);
                    Debug.WriteLine("Cathed Inner exception: " + ex?.InnerException?.Message);
                    Debug.WriteLine("Cathed Inner Inner exception: " + ex?.InnerException?.InnerException?.Message);
                    Cart cart;

                    var CartCREATEDorACTIVE = user.Carts.Where(crt => crt.cartStatus == CartStatus.CREATED.ToString() || crt.cartStatus == CartStatus.ACTIVE.ToString());
                    *//*if (CartCREATEDorACTIVE.Count() > 0)
                    {*//*
                        var LastCartCREATEDorACTIVE = CartCREATEDorACTIVE.LastOrDefault();
                        cart = LastCartCREATEDorACTIVE;
                        *//*db.Carts.AddOrUpdate(crt => crt.cid != LastCartCREATEDorACTIVE.cid && crt.cartStatus == CartStatus.CREATED.ToString(), )*/

                        
                    /*}
                    else
                    {
                        cart = CreateNewCart(user.uid);
                    }*//*


                    cart.cartStatus = CartStatus.ACTIVE.ToString();
                    db.Carts.AddOrUpdate(cart);
                    db.SaveChanges();

                    var cartItem = new CartItem();
                    cartItem.cid = cart.cid;
                    cartItem.pid = pid;
                    cartItem.pName = product.name;
                    cartItem.pShortDesc = product.shortDesc;
                    cartItem.pAmount = product.amount;
                    cartItem.pBuyQty = buyQty;

                    if (cart.CartItems.Any(crtItem => crtItem.Product.pid == pid))
                    {
                        var existingCartItem = cart.CartItems.Where(crtItem => crtItem.Product.pid == pid).FirstOrDefault();
                        existingCartItem.pBuyQty = buyQty;
                        db.CartItems.AddOrUpdate(existingCartItem);
                    }
                    else
                    {
                        CartItem newCartItem = new CartItem();
                        newCartItem.cid = cart.cid;
                        newCartItem.pid = pid;
                        newCartItem.pName = product.name;
                        newCartItem.pShortDesc = product.shortDesc;
                        newCartItem.pAmount = product.amount;
                        newCartItem.pBuyQty = buyQty;
                        db.CartItems.Add(newCartItem);
                    }

                    db.CartItems.Add(cartItem);
                    db.SaveChanges();

                }*/
            }
            else
            {
                Debug.WriteLine("No Cart Present");

                Cart newCartOpen = CreateNewCart(uid);

                db.Carts.Add(newCartOpen);
                if (db.SaveChanges() > 0)
                {
                    /*EnvatoDBEntities db1 = new EnvatoDBEntities();*/
                    /*Cart newCartCreated = db.Carts.LastOrDefault(crt => crt.uid == uid);*/

                    CartItem newCartItem = new CartItem();
                    newCartItem.cid = newCartOpen.cid;
                    newCartItem.pid = pid;
                    newCartItem.pName = product.name;
                    newCartItem.pShortDesc = product.shortDesc;
                    newCartItem.pAmount = product.amount;
                    newCartItem.pBuyQty = buyQty;

                    db.CartItems.Add(newCartItem);
                    db.Products.AddOrUpdate(product);
                    db.SaveChanges();
                }
            }



            return RedirectToAction("Index");
        }

        public static Cart CreateNewCart(int uid)
        {
            Cart newCartOpen = new Cart();
            newCartOpen.uid = uid;
            newCartOpen.createdAt = DateTime.Now;
            newCartOpen.cartStatus = CartStatus.CREATED.ToString();
            return newCartOpen;
        }

        private PaymentType getPaymentType(string paymentType)
        {
            if (paymentType == "Cash")
            {
                return PaymentType.Cash;
            }
            else
            {
                return PaymentType.Card;
            }
        }

        public ActionResult Checkout(int? id, string PaymentType)
        {
            PaymentType paymentType = getPaymentType(PaymentType);
            var cart = db.Carts.Where(crt => crt.cid == id).FirstOrDefault();
            cart.cartStatus = CartStatus.DELIVERED.ToString();
            cart.closedAt = DateTime.Now;
            cart.payid= (int)paymentType;

            db.Carts.AddOrUpdate(cart);

            Cart newCart = CreateNewCart(cart.uid);
            db.Carts.Add(newCart);
            if(db.SaveChanges() > 0)
            {
                return RedirectToAction("OrderPlaced");
            }

            
            return RedirectToAction("Index", "Home");
        }

        public ActionResult OrderPlaced()
        {
            return View();
        }
    }
}