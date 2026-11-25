window.showToastr = function (type, message) {
    if (type == "success") {

        toastr.success(message);
    }
    if (type == "error") {
        toastr.error(message);
    }
    if (type == "warning") {
        toastr.warning(message);
    } 
}

toastr.options = {
    "timeOut": "5000",
    "closeButton": true,
    "progressBar": true
};

window.ShowSwal = function (type, message) {
    return new Promise((test) => {
        if (type == "success") {
            Swal.fire({
                title: message,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "ตกลง"
            }).then((result) => {
                if (result.isConfirmed) {
                    test(true);
                } else {
                    test(false);
                }
            });
        }
        else if (type == "error") {
            Swal.fire({
                title: "โอ๊ะ! มีบางอย่างผิดพลาด!",
                text: message,
                icon: "error"
            }).then(() => {
                test(false);
            });
        }
        else if (type == "test") {
            Swal.fire({
                icon: "success",
                title: message,
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                test(false);
            });
        }
        else if (type == "confirm") {
            Swal.fire({
                title: "ข้อมูลซ้ำ",
                text: message,
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "บันทึกซ้ำ",
                cancelButtonText: "ยกเลิก"
            }).then((result) => {
                test(result.isConfirmed);
            });
        }
        else if (type == "warning") {
            Swal.fire({
                icon: "warning",
                title: message,
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                test(false);
            });
        }
    });
};

window.downloadPdfFile = (base64Data, fileName) => {
    const link = document.createElement('a');
    link.href = 'data:application/pdf;base64,' + base64Data;
    link.download = fileName;
    link.click();
};


window.downloadExcelFile = (binaryData, fileName) => {
    const blob = new Blob([binaryData], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const link = document.createElement('a');
    link.href = URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
};

function downloadFileFromBase64(base64, fileName, contentType) {
    const link = document.createElement('a');
    link.href = `data:${contentType};base64,${base64}`;
    link.download = fileName;
    link.click();
}