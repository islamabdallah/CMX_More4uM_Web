'use strict';
function loadDetails(id, name, subCount) {
    var Id = parseInt(id);
    $.ajax({
        url: "https://more4u.cemex.com.eg/more4u/more4u/Medical/GetSubCategories2",
        data: { Id: Id },
        type: "POST",
        async: true,
        success: function (data) {
            if (data != null) {
                var dataJSON = JSON.parse(data);
                var medicalSub = document.getElementById("medicalSub");
                if (medicalSub != null) {
                    medicalSub.innerHTML = "";
                    document.getElementById("categoryTitle").innerText = dataJSON['MedicalSubCategoryModels'][0]['Name_EN']; //name;
                    document.getElementById("subCount").innerText = subCount;
                    if (dataJSON["MedicalSubCategoryModels"].length == 1) {
                        for (var index = 0; index < dataJSON["MedicalSubCategoryModels"][0]["MedicalDetailsCount"]; index++) {
                            var li = document.createElement("li");
                            li.setAttribute("class", "list-group-item border-0 d-flex justify-content-between ps-0 mb-1 border-radius-lg");
                            var DetailsDiv = document.createElement("div");
                            DetailsDiv.setAttribute("class", "d-flex align-items-center text-sm align-middle");
                            var DetailsAncor = document.createElement("a");
                            DetailsAncor.setAttribute("class", "text-body text-xs font-weight-bold align-items-center align-middle");
                            DetailsAncor.setAttribute("href", "javascript:;");
                            DetailsAncor.setAttribute("onclick", "GetMedicalDetails(" + dataJSON['MedicalSubCategoryModels'][0]['MedicalDetailsModels'][index]['Id'] + ",'" + name + "')");
                            DetailsAncor.innerText = "Details";
                            DetailsAncor.innerHTML = 'Details <i class="fas fa-arrow-right text-xs ms-1" aria-hidden="true"></i>';
                            DetailsDiv.append(DetailsAncor);
                            var Namediv = document.createElement("div");
                            Namediv.setAttribute("class", "d-flex flex-column ");
                            var Namehead = document.createElement("h5");
                            Namehead.setAttribute("class", "mb-1 text-dark font-weight-bold text-m text-center");
                            Namehead.innerText = dataJSON["MedicalSubCategoryModels"][0]["MedicalDetailsModels"][index]["Name_AR"];
                            Namediv.append(Namehead);
                            var CatSpan = document.createElement("span");
                            CatSpan.setAttribute("class", "text-xs");
                            CatSpan.innerText = "#" + name;
                            Namediv.append(CatSpan);
                            li.append(Namediv);
                            li.append(DetailsDiv);
                            medicalSub.append(li);
                        }
                    }
                }
            }
            else {
                swal("Failed process. Load Details",
                    {
                        icon: "error",
                    });
            }
        },
        error: function () {
            swal("Failed process.Load Details",
                {
                    icon: "error",
                });
        }

    });

}


function loadDetailsfromSubCategory(id, name, subCount) {
    var Id = parseInt(id);
    $.ajax({
        url: "https://more4u.cemex.com.eg/more4u/more4u/Medical/GetAllMedicalDetailsForSubCategory",
        data: { Id: Id },
        type: "POST",
        async: true,
        success: function (data) {
            if (data != null) {
                var dataJSON = JSON.parse(data);
                var medicalSub = document.getElementById("medicalSub");
                if (medicalSub != null) {
                    medicalSub.innerHTML = "";
                    document.getElementById("categoryTitle").innerText = name;
                    document.getElementById("subCount").innerText = subCount;
                    if (dataJSON["MedicalDetailsModels"] != null) {
                        for (var index = 0; index < dataJSON["MedicalDetailsModels"].length; index++) {
                            var li = document.createElement("li");
                            li.setAttribute("class", "list-group-item border-0 d-flex justify-content-between ps-0 mb-1 border-radius-lg");
                            var DetailsDiv = document.createElement("div");
                            DetailsDiv.setAttribute("class", "d-flex align-items-center text-sm align-middle");
                            var DetailsAncor = document.createElement("a");
                            DetailsAncor.setAttribute("class", "text-body text-xs font-weight-bold align-items-center align-middle");
                            DetailsAncor.setAttribute("href", "javascript:;");
                            DetailsAncor.setAttribute("onclick", "GetMedicalDetails(" + dataJSON['MedicalDetailsModels'][index]['Id'] + ")");
                            DetailsAncor.innerText = "Details";
                            DetailsAncor.innerHTML = 'Details <i class="fas fa-arrow-right text-xs ms-1" aria-hidden="true"></i>';
                            DetailsDiv.append(DetailsAncor);
                            var Namediv = document.createElement("div");
                            Namediv.setAttribute("class", "d-flex flex-column ");
                            var Namehead = document.createElement("h6");
                            Namehead.setAttribute("class", "mb-1 text-dark font-weight-bold text-sm text-center");
                            Namehead.innerText = dataJSON["MedicalDetailsModels"][index]["Name_AR"];
                            Namediv.append(Namehead);
                            var CatSpan = document.createElement("span");
                            CatSpan.setAttribute("class", "text-xs");
                            CatSpan.innerText = "#" + name;
                            Namediv.append(CatSpan);
                            li.append(Namediv);
                            li.append(DetailsDiv);
                            medicalSub.append(li);
                        }
                    }
                }
            }
            else {
                swal("Failed process, loadDetailsfromSubCategory",
                    {
                        icon: "error",
                    });
            }
        },
        error: function () {
            swal("Failed process,loadDetailsfromSubCategory",
                {
                    icon: "error",
                });
        }

    });

}

function GetMedicalDetails2(id) {
    var Id = parseInt(id);
    $.ajax({
        url: "https://more4u.cemex.com.eg/more4u/more4u/Medical/GetMedicalDetailsAsync",
        data: { detailsId: Id },
        type: "POST",
        async: true,
        success: function (data) {
            if (data != null) {
                var detailsJSON = JSON.parse(data);
                var medicalDetaildList = document.getElementById("medicalDetaildList");
                if (medicalDetaildList != null) {
                    var phonesSplitted = (detailsJSON["Mobile"]).split(";");
                    var nameHead = document.getElementById("medicalDetailsName");
                    nameHead.innerText = detailsJSON["Name_AR"];
                    var phoneDetails = document.getElementById("phoneDetails");
                    if (phonesSplitted.length == 1) {
                        var PhoneSpan = document.getElementById("span");
                        PhoneSpan.setAttribute("class", "text-xxs pr-2");
                        PhoneSpan.setAttribute("display", "block");
                        PhoneSpan.innerHTML = phonesSplitted[0];
                        phoneDetails.append(PhoneSpan);
                    }
                    else {
                        for (var phoneIndex = 0; phoneIndex < phonesSplitted.length; phoneIndex++) {
                            var PhoneSpan = document.createElement("span");
                            PhoneSpan.setAttribute("class", "text-xxs");
                            PhoneSpan.setAttribute("style", "padding-right:10px");
                            PhoneSpan.setAttribute("display", "block");
                            PhoneSpan.innerHTML = phonesSplitted[index];
                            phoneDetails.append(PhoneSpan);
                        }
                    }
                    $('#detailsCard').modal('show');
                }
                else {
                    swal("Failed process, GetMedicalDetails2",
                        {
                            icon: "error",
                        });
                }
            }
        },
        error: function () {
            swal("Failed process, GetMedicalDetails2",
                {
                    icon: "error",
                });
        }

    });
}

function GetMedicalDetails(id, name) {
    var Id = parseInt(id);
    $.ajax({
        url: "https://more4u.cemex.com.eg/more4u/more4u/Medical/GetMedicalDetails2",
        //url: "GetMedicalDetails2",
        data: { detailsId: Id },
        type: "POST",
        async: true,
        success: function (data) {
            if (data != null) {
                var detailsJSON = JSON.parse(data);
                document.getElementById("modalTitle").innerText = name;
                document.getElementById("bio").innerText = "#" + name;
                var medicalDetaildList = document.getElementById("medicalDetaildList");
                if (medicalDetaildList != null) {
                    document.getElementById("detailsImage").src = detailsJSON["Image"];
                    var phonesSplitted = (detailsJSON["Mobile"]).split("&");
                    var addressSplitted = (detailsJSON["Address_AR"]).split("&");
                    var datesSplitted = (detailsJSON["WorkingHours_AR"]).split("&");
                    var nameHead = document.getElementById("detailsName");
                    nameHead.innerText = detailsJSON["Name_AR"];
                    var phoneDetails = document.getElementById("detailsPhones");
                    var AddressDetails = document.getElementById("detailsAddress");
                    AddressDetails.innerHTML = "";
                    phoneDetails.innerHTML = "";
                    var addressHead = document.createElement("h6");
                    addressHead.setAttribute("class", "mb-0 text-sm");
                    addressHead.innerHTML = '<i class="fa fa-map-marker" style="padding-right:4px" aria-hidden="true"></i>' + "Address";
                    AddressDetails.append(addressHead);
                    var phoneHead = document.createElement("h6");
                    phoneHead.setAttribute("class", "mb-0 text-sm");
                    phoneHead.innerHTML = '<i class="fa fa-phone-square" style="padding-right:4px" aria-hidden="true"></i>' + "Phone";
                    phoneDetails.append(phoneHead);
                    if (phonesSplitted.length == 1) {
                        var PhoneSpan = document.createElement("span");
                        PhoneSpan.setAttribute("class", "text-xxs pr-2");
                        PhoneSpan.setAttribute("style", "display:block");
                        PhoneSpan.innerHTML = phonesSplitted[0];
                        phoneDetails.append(PhoneSpan);
                    }
                    else {
                        for (var phoneIndex = 0; phoneIndex < phonesSplitted.length; phoneIndex++) {
                            var PhoneSpan = document.createElement("span");
                            PhoneSpan.setAttribute("class", "text-xxs");
                            PhoneSpan.setAttribute("style", "padding-right:10px; display:block");
                            PhoneSpan.innerHTML = phonesSplitted[phoneIndex];
                            phoneDetails.append(PhoneSpan);
                        }
                    }

                    if (addressSplitted.length == 1) {
                        var AddressSpan = document.createElement("p");
                        AddressSpan.setAttribute("class", "mb-0 text-xs");
                        AddressSpan.setAttribute("style", "display:block");
                        AddressSpan.innerHTML = addressSplitted[0]
                        AddressDetails.append(AddressSpan);
                    }
                    else {
                        for (var phoneIndex = 0; phoneIndex < phonesSplitted.length; phoneIndex++) {
                            var AddressSpan = document.createElement("p");
                            AddressSpan.setAttribute("class", "mb-2 text-xs");
                            //AddressSpan.setAttribute("style", "padding-right:10px; display:block");
                            AddressSpan.innerHTML = addressSplitted[phoneIndex];
                            AddressDetails.append(AddressSpan);
                        }
                    }
                    var DatesDetails = document.getElementById("detailsWorkingHours");
                    if (DatesDetails != null) {
                        DatesDetails.innerHTML = "";
                        var dateHead = document.createElement("h6");
                        dateHead.setAttribute("class", "mb-0 text-sm");
                        dateHead.innerHTML = '<i class="fa fa-calendar" style="padding-right:4px" aria-hidden="true"></i>' + "Working Hours";
                        DatesDetails.append(dateHead);
                        if (datesSplitted.length == 1) {
                            var DatesSpan = document.createElement("p");
                            DatesSpan.setAttribute("class", "mb-0 text-xs");
                            //DatesSpan.setAttribute("style", "display:block");
                            DatesSpan.innerHTML = datesSplitted[0]
                            DatesDetails.append(DatesSpan);
                        }
                        else {
                            for (var phoneIndex = 0; phoneIndex < phonesSplitted.length; phoneIndex++) {
                                var DatesSpan = document.createElement("p");
                                DatesSpan.setAttribute("class", "mb-2 text-xs");
                                //AddressSpan.setAttribute("style", "padding-right:10px; display:block");
                                DatesSpan.innerHTML = datesSplitted[phoneIndex];
                                DatesDetails.append(DatesSpan);
                            }
                        }
                    }

                    $('#detailsCard').modal('show');
                }
                else {
                    swal("Failed process, GetMedicalDetails",
                        {
                            icon: "error",
                        });
                }
            }

        },
        error: function () {
            swal("Failed process, GetMedicalDetails",
                {
                    icon: "error",
                });
        }

    });
}

