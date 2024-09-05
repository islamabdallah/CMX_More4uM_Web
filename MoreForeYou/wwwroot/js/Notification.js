var userId = document.getElementById("xyzId").value;
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationHub?userid=" +userId).build();
connection.on("sendToUser", (type, date, time, benefitId, message, employeeName, employeeUserId) => {
    alert("testaaaaaaa22");
    var count = parseInt(document.getElementById("notificationCount1").textContent);
    document.getElementById("notificationCount1").textContent = count + 1;
    document.getElementById("notificationCount2").textContent = count + 1;
    var li = document.createElement("li");
    li.setAttribute("class", "notification-item");
    var i = document.createElement("i");
    if (type == "Request" || type == "Take Action") {
        i.setAttribute("class", "bi bi-question-circle");
        i.setAttribute("style", "font-size: xx-large; color:blue");
    }
    else if (type == "Gift") {
        i.setAttribute("class", "bi bi-gift");
        i.setAttribute("style", "color: hotpink; font-size: xx-large")
    }
    else if (type == "Response" || type == "CreateGroup") {
        if (message.contains("Approved")) {
            i.setAttribute("class", "bi bi-emoji-heart-eyes");
            i.setAttribute("style", "color:lawngreen; font-size:xx-large");

        }
        else if (message.contains("Rejected")) {
            i.setAttribute("class", "bi bi-emoji-frown");
            i.setAttribute("style", "color:red; font-size:xx-large");

        }
        else {
            i.setAttribute("class", "bx bx-user-plus");
            i.setAttribute("style", "color:blue; font-size:xx-large");
        }

    }
    else if (type == "RequestCancel") {
        i.setAttribute("class", "bi bi-x-circle");
        i.setAttribute("style", "color:red; font-size: xx-large");
    }

    var div = document.createElement("div");
    var h4 = document.createElement("h4");
    h4.textContent = type;
    var pMessage = document.createElement("p");
    pMessage.textContent = message;
    var pDateTime = document.createElement("p");
    pDateTime.setAttribute("style", "color:black");
    p.textContent = date + ", " + time;
    div.appendChild(h4);
    div.appendChild(pMessage);
    div.appendChild(pDateTime);
    li.appendChild(i);
    li.appendChild(div);
    var ul = document.getElementById("notificationList");
    ul.appendChild(li);
    var liDivider = document.createElement("li");
    liDivider.setAttribute("class", "dropdown-divider");
    ul.appendChild(liDivider);
    });
//connection.start();
connection.start().catch(function (err)
{
    return console.error(err.toString());
}).then(function ()
{
    connection.invoke('GetConnectionId').then(function (connectionId) {
    });
});