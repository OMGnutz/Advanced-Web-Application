var products = document.getElementsByClassName("indicontainer")
var prices = document.getElementsByClassName("lblpriceclass")
for (let el = 0; el < products.length; el++) {
    prices[el].style.color = "white";
}

for (let el = 0; el < products.length; el++) {
    var discountperc = products[el].getElementsByClassName("discountperc")[0]
    var discountprice = products[el].getElementsByClassName("lbldiscount")[0]
    var originalprice = products[el].getElementsByClassName("lblpriceclass")[0];

    if (discountprice.innerText == "$") {
        discountprice.style.display = "none";
        discountperc.style.display = "none";
    }
    else {
        var string = new String(originalprice.innerText);
        originalprice.innerHTML = string.strike();
        originalprice.style.color = "grey";
        discountprice.style.display = "block";
        discountprice.style.color = "white";
        let percentage = 100 - discountprice.textContent.replace('$' , '')  / (originalprice.textContent.replace('$' , '') / 100);
        discountperc.textContent = "-" + new String(percentage) + "%";
    }
}