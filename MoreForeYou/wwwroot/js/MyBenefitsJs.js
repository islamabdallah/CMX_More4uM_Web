'use strict';
function getBenefitRequests(id, requestNumber) {
    var Id = parseInt(id);
    var RequestNumber = parseInt(requestNumber);
    $.ajax({
        url: "GetMyBenefitRequests2",
        data: { BenefitId: Id, requestNumber: RequestNumber },
        type: "POST",
        async: true,
        success: function (data) {
            if (data != null) {
                var dataJSON = JSON.parse(data);
                var requestsCard = document.getElementById("requestsCard");
                requestsCard.innerHTML = "";
                var title = document.getElementById("title");
                title.innerText = "Requests history";
                var lmtitle = document.getElementById("lmtitle");
                lmtitle.innerText = dataJSON["Requests"][0]["BenefitName"];
                var RequestsCount = document.getElementById("RequestsCount");
                RequestsCount.innerText = dataJSON["Requests"].length;
                var flag = 0;
                var statusId = "";
                for (var index = 0; index < dataJSON["Requests"].length; index++) {
                    var requestDate = new Date(dataJSON["Requests"][index]["Requestedat"]).toLocaleDateString();
                    var requiredDate = new Date(dataJSON["Requests"][index]["From"]).toLocaleDateString();
                    var statusSpan = document.createElement("span");

                    var recentI = document.createElement("i");
                    var recentButton = document.createElement("button");
                    var recentRequestNumber = document.createElement("h6");
                    recentRequestNumber.setAttribute("class", "mb-1 text-dark text-sm");
                    recentI.setAttribute("class", "fas fa-arrow-up");
                    var recentCancelButton = document.createElement("a");
                    recentCancelButton.setAttribute("class", "btn btn-link text-dark text-gradient px-1 mb-0");
                    var recentEditButton = document.createElement("a");
                    recentEditButton.setAttribute("class", "btn btn-link text-dark text-gradient px-1 mb-0");
                    var x = dataJSON["Requests"][index];
                    recentEditButton.setAttribute("onclick", 'showRequestDetails(' + data + ',' + index + ')');
                    if (dataJSON["Requests"][index]["CanCancel"] == false) {
                        recentCancelButton.setAttribute("style", "pointerEvents :none; cursor: default; opacity: 0.4");
                        recentCancelButton.setAttribute("title", "You can't cancel this benefit");
                    }
                    else {
                        recentCancelButton.setAttribute("id", dataJSON["Requests"][index]["RequestNumber"]);
                        recentCancelButton.setAttribute("onclick", 'cancelMyRequest(' + dataJSON["Requests"][index]["RequestNumber"] + ',' + dataJSON["Requests"][index]["benefitId"] + ')');
                        recentCancelButton.setAttribute("href", "javascript:;");
                        statusId = 'S' + parseInt(dataJSON["Requests"][index]["RequestNumber"]);
                       // alert(statusId);
                       // statusSpan.setAttribute("id", statusId);
                        statusSpan.setAttribute("id", "lmstatus");
                        recentButton.setAttribute("id", "lmrecent");
                    }

                    if (dataJSON["Requests"][index]["status"] == "Pending") {
                        statusSpan.setAttribute("class", "badge badge-sm bg-gradient-info pr-5");
                       // statusSpan.setAttribute("id", "lmstatus");
                        statusSpan.innerText = "Pending";
                        recentButton.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-info mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");

                        recentRequestNumber.innerHTML = '# ' + '<span style="margin-right:5px">' + dataJSON["Requests"][index]["RequestNumber"] +
                            '</span><span class="badge badge-sm bg-gradient-info pr-5">Panding</span>';

                    }
                    else if (dataJSON["Requests"][index]["status"] == "Approved") {
                        statusSpan.setAttribute("class", "badge badge-sm bg-gradient-success pr-5");
                        //statusSpan.setAttribute("id", "lmstatus");
                        statusSpan.innerText = "Approved";
                        recentButton.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-success mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");
                        recentRequestNumber.innerHTML = '# ' + '<span style="margin-right:5px">' + dataJSON["Requests"][index]["RequestNumber"] +
                            '</span><span class="badge badge-sm bg-gradient-success pr-5">Approved</span>';
                    }
                    else if (dataJSON["Requests"][index]["status"] == "Rejected") {
                        statusSpan.setAttribute("class", "badge badge-sm bg-gradient-danger pr-5");
                       // statusSpan.setAttribute("id", "lmstatus");
                        statusSpan.innerText = "Rejected";
                        recentButton.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-danger mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");
                        recentRequestNumber.innerHTML = '# ' + '<span style="margin-right:5px">' + dataJSON["Requests"][index]["RequestNumber"] +
                            '</span><span class="badge badge-sm bg-gradient-danger pr-5">Rejected</span>';
                    }
                    else if (dataJSON["Requests"][index]["status"] == "Cancelled") {
                        statusSpan.setAttribute("class", "badge badge-sm bg-gradient-dark pr-5 ml-2");
                        statusSpan.innerText = "Cancelled";
                        recentButton.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-dark mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");
                        recentRequestNumber.innerHTML = '# ' + '<span style="margin-right:5px">' + dataJSON["Requests"][index]["RequestNumber"] +
                            '</span><span class="badge badge-sm bg-gradient-dark pr-5">Cancelled</span>';
                    }
                    else if (dataJSON["Requests"][index]["status"] == "InProgress") {
                        statusSpan.setAttribute("class", "badge badge-sm bg-gradient-warning pr-5");
                        statusSpan.innerText = "InProgress";
                        recentButton.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-warning mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");
                        recentRequestNumber.innerHTML = '# ' + '<span style="margin-right:5px">' + dataJSON["Requests"][index]["RequestNumber"] +
                            '</span><span class="badge badge-sm bg-gradient-warning pr-5">InProgress</span>';
                    }
                    if (index == 0) {
                        var head = document.createElement("h6");
                        head.setAttribute("class", "text-uppercase text-body text-xs font-weight-bolder mb-3");
                        head.innerText = "Recently";
                        requestsCard.append(head);
                    }
                    else if (flag == 0 && index > 0) {
                        var head = document.createElement("h6");
                        head.setAttribute("class", "text-uppercase text-body text-xs font-weight-bolder mb-3");
                        head.innerText = "Previous";
                        requestsCard.append(head);
                        flag = 1;
                    }
                    var recentUL = document.createElement("ul");
                    recentUL.setAttribute("class", "list-group");
                    var recentLi = document.createElement("li");
                    recentLi.setAttribute("class", "list-group-item border-0 d-flex justify-content-between ps-0 mb-2 border-radius-lg");
                    var recentDiv = document.createElement("div");
                    recentDiv.setAttribute("class", "d-flex align-items-center");
                    var recentDetailsDiv = document.createElement("div");
                    recentDetailsDiv.setAttribute("class", "d-flex flex-column");
                    var recentRequests_RequestedAt = document.createElement("sapn");
                    recentRequests_RequestedAt.setAttribute("class", "text-xs");
                    var recentRequests_RequiredAt = document.createElement("sapn");
                    recentRequests_RequiredAt.setAttribute("class", "text-xs");
                    var recentButtonsDiv = document.createElement("div");
                    recentButtonsDiv.setAttribute("class", "d-flex align-items-center text-danger text-gradient text-sm font-weight-bold");
                    var recentButtonsDivInside = document.createElement("div");
                    recentButtonsDivInside.setAttribute("class", "ms-auto text-end align-middle");
                    recentRequests_RequestedAt.innerText = "Reuested At :-  " + requestDate;
                    recentRequests_RequiredAt.innerText = "Required At :-  " + requiredDate;
                    recentCancelButton.innerHTML = '<i class="far fa-trash-alt me-2"></i>' + ' Cancel';
                    recentEditButton.innerHTML = '<i class="far fa-eye me-2"></i>' + 'View';
                    recentButtonsDivInside.append(recentCancelButton);
                    recentButtonsDivInside.append(recentEditButton);
                    recentButtonsDiv.append(recentButtonsDivInside);
                    recentDetailsDiv.append(recentRequestNumber);
                    recentDetailsDiv.append(recentRequests_RequestedAt);
                    recentDetailsDiv.append(recentRequests_RequiredAt);
                    recentButton.append(recentI);
                    recentDiv.append(recentButton);
                    recentDiv.append(recentDetailsDiv);
                    recentLi.append(recentDiv);
                    recentLi.append(recentDiv);
                    recentLi.append(recentDiv);
                    recentLi.append(recentButtonsDiv);
                    recentUL.appendChild(recentLi);
                    requestsCard.append(recentUL);

                }
            }
        },
        error: function () {
            swal("Failed process,Can't show details",
                {
                    icon: "error",
                });
        }


    });
}


function cancelMyRequest(requestNumber, benefitId) {
    var ele = document.getElementById(requestNumber);
    var eleStatus = document.getElementById("status" + requestNumber);
    benefitId = parseInt(benefitId);
    requestNumber = parseInt(requestNumber);
    $.ajax({
        url: "RequestCancel2",
        data: { BenefitId: benefitId, id: requestNumber },
        type: "POST",
        async: true,
        success: function (data)
        {
            if (data == true) {
                swal("Successful process, Your request has been cancelled",
                    {
                        icon: "success",
                    });

                ele.setAttribute("onclick", "");
                ele.setAttribute("style", "pointerEvents :none; cursor: default; opacity: 0.4");
                ele.setAttribute("title", "You can't cancel this benefit");
                eleStatus.innerText = "Cancelled";
                eleStatus.setAttribute("class", "badge badge-sm bg-gradient-dark pr-5 ml-2");

                var lmele = document.getElementById("lmstatus");
                var lmeleStatus = document.getElementById("lmrecent");
                lmele.setAttribute("class", "badge badge-sm bg-gradient-dark pr-5 ml-2");
                lmele.innerText = "Cancelled";
                lmeleStatus.setAttribute("class", "btn btn-icon-only btn-rounded btn-outline-dark mb-0 me-3 btn-sm d-flex align-items-center justify-content-center");
               }
        },
        error: function () {
            swal("Failed process, Problem in  Adding",
                {
                    icon: "error",
                });
        }
    });
}

function showRequestDetails(data, index) {
    if (data != null) {
        var requestStatus = "";
        document.getElementById("requestBenefitName").innerText = data["Requests"][index]["BenefitName"];
        document.getElementById("requestEmployeeName").innerText = data["Requests"][index]["CreatedBy"]["UserName"];
        document.getElementById("requestFrom").innerText = data["Requests"][index]["From"];
        document.getElementById("requestTo").innerText = data["Requests"][index]["To"];
        document.getElementById("requestBenefitType").innerText = data["Requests"][index]["BenefitType"];
        document.getElementById("requestStatus").innerText = data["Requests"][index]["status"];
        document.getElementById("benefitImage").src = data["Requests"][index]["BenefitCardAPI"];
        if (data["Requests"][index]["RequestWorkFlowAPIs"] != null) {
            var requestWorkflowTimeline = document.getElementById("requestWorkflowTimeline");
            requestWorkflowTimeline.innerHTML = "";
            for (var flowIndex = 0; flowIndex < data["Requests"][index]["RequestWorkFlowAPIs"].length; flowIndex++) {
                requestStatus = data["Requests"][index]["RequestWorkFlowAPIs"][flowIndex]["Status"];
                var timlineDiv = document.createElement("div");
                timlineDiv.setAttribute("class", "timeline-block mb-3");
                var timelineStepSpan = document.createElement("span");
                timelineStepSpan.setAttribute("class", "timeline-step");
                var timelineContentDiv = document.createElement("div");
                timelineContentDiv.setAttribute("class", "timeline-content");
                var headName = document.createElement("h6");
                headName.setAttribute("class", "text-dark text-sm font-weight-bold mb-0");
                var paragraphDate = document.createElement("p");
                paragraphDate.setAttribute("class", "text-secondary font-weight-bold text-xs mt-1 mb-0");
                var paragraphReplay = document.createElement("p");
                paragraphReplay.setAttribute("class", "text-secondary font-weight-bold text-xs mt-1 mb-0");
                headName.innerText = data["Requests"][index]["RequestWorkFlowAPIs"][flowIndex]["UserName"];
                if (requestStatus == "Approved" || requestStatus == "Rejected") {
                    paragraphDate.innerText = new Date(data["Requests"][index]["RequestWorkFlowAPIs"][flowIndex]["ReplayDate"]).toLocaleDateString();
                    paragraphReplay.innerText = data["Requests"][index]["RequestWorkFlowAPIs"][flowIndex]["Notes"];
                }
                if (requestStatus == "Pending") {
                    timelineStepSpan.innerHTML = '<i class="fa fa-envelope text-info text-gradient"></i>';
                }
                else if (requestStatus == "InProgress") {
                    timelineStepSpan.innerHTML = '<i class="fa fa-spinner text-warning text-gradient"></i>';
                }
                else if (requestStatus == "Approved") {
                    timelineStepSpan.innerHTML = '<i class="fa fa-check text-warning text-gradient"></i>';
                }
                else if (requestStatus == "Rejected") {
                    timelineStepSpan.innerHTML = '<i class="fa fa-times text-danger text-gradient"></i>';
                }
                //else if (requestStatus == "Cancelled") {
                //    timelineStepSpan.innerHTML = '<i class="ni ni-bell-55 text-dark text-gradient"></i>';
                //}
                timelineContentDiv.append(headName);
                timelineContentDiv.append(paragraphDate);
                timelineContentDiv.append(paragraphReplay);

                timlineDiv.append(timelineStepSpan);
                timlineDiv.append(timelineContentDiv);
                requestWorkflowTimeline.append(timlineDiv);

            }
        }
        $('#requestDetails').modal('show');
    }
}