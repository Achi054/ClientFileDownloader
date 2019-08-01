(function () {
    var selectedAttachments = '';

    // Attachment download click event
    $('#file-download').on('click', function () {
        if (validInput()) {
            $.getJSON("https://localhost:44393/api/file-download/attachments/download?" + selectedAttachments,
                function (data) {
                    $.each(data.attachments, function (i, v) {
                        downloadAttachments(v)
                    });

                    showMissingAttachments(data.missingAttachments);
                });
        }
    });

    // Attachment download as zip click event
    $('#zip-download').on('click', function () {
        if (validInput()) {
            $.getJSON("https://localhost:44393/api/file-download/attachments/downloadzip?" + selectedAttachments,
                function (data) {
                    var attachment = data.attachments[0];
                    downloadAttachments(attachment)
                    showMissingAttachments(data.missingAttachments);
                });
        }
    });

    // Render the Kendo grid with attachments
    $("#attachment-grid").kendoGrid({
        dataSource: {
            pageSize: 10,
            transport: {
                read: {
                    url: "https://localhost:44393/api/file-download/attachments",
                    dataType: "json"
                }
            },
            schema: {
                model: {
                    id: "id",
                    fields: {
                        name: { type: "string" }
                    }
                }
            }
        },
        persistSelection: true,
        change: onAttachmentSelect,
        columns: [
            { selectable: true, width: "50px" },
            { field: "name", title: "Name" }]
    });

    // Change event handler on checkbox select
    function onAttachmentSelect() {
        $('#missing-files').text('');
        selectedAttachments = '';
        var selectedKey = this.selectedKeyNames();
        $.each(selectedKey, function (i, v) {
            selectedAttachments += "attachmentIds=" + parseInt(v) + "&";
        });
        selectedAttachments = selectedAttachments.substring(0, selectedAttachments.length - 1);
    }

    // User Message functionality
    function showMissingAttachments(missingAttachments) {
        if (missingAttachments && missingAttachments.length > 0) {
            var missingFileNames = '';
            $.each(missingAttachments, function (i, v) {
                missingFileNames += v.name + ', ';
            });
            missingFileNames = missingFileNames.substring(0, missingFileNames.length - 2);
            $('#missing-files').text("Attachment(s) " + missingFileNames + " are missing.");
        }
    }

    // Kendo UI client downloader
    function downloadAttachments(attachment) {
        var dataURI = "data:" + attachment.mimeType + "" + attachment.base64 + "," + attachment.content;
        kendo.saveAs({
            dataURI: dataURI,
            fileName: attachment.name
        });
    }

    // Validate input
    function validInput() {
        if (!$('.k-checkbox').is(':checked')) {
            $('#missing-files').text('Please select a attachment.');
            return false
        }
        return true;
    }
})();