
async function SendFile(apiBaseURL, callbackOK, callbackERR) {

    // Config params
    var targetRoundTrip = 2;  // seconds
    var chunkSizeMin = 10 * 1024;
    var chunkSizeMax = 2 * 1024 * 1024;
    var maxRetriesOnError = 3;

    // UI elements
    var uploadGuid = document.getElementById("UploadGuid").value;
    var fileInput = document.getElementById("FileInput");
    var divProgress = document.getElementById("divProgress");
    var divProgressBar = document.getElementById("divProgressBar");
    var divProgressBarValue = document.getElementById("divProgressBarValue");
    var dirProgressText = document.getElementById("dirProgressText");

    // exit if no file selected
    if (fileInput.files.length == 0)
        return;

    // init UI
    document.getElementById("buttonSubmit").style.visibility = "hidden";
    divProgress.style.visibility = "visible";
    divProgressBarValue.style.width = "0px";
    dirProgressText.innerText = "";

    // Process file
    var sourceFile = fileInput.files[0];
    var chunkSize = chunkSizeMin;
    var startPos = 0;
    var endPos = chunkSize;
    var errorCounter = 0;
    var progressBarWidth = parseInt(divProgressBar.style.width, 10);

    // upload chunks cycle
    while (startPos < sourceFile.size) {

        console.log("_CHUNK_");

        // progress bar update
        divProgressBarValue.style.width = Math.floor(startPos * progressBarWidth / sourceFile.size) + "px";

        // get chunk from file
        console.log("Read from file: startPos=" + startPos + " chunkSize=" + chunkSize);
        var endPos = startPos + chunkSize;
        var chunkBlob = sourceFile.slice(startPos, endPos);

        // Send data                       
        var fullURL = apiBaseURL + "?uploadguid=" + uploadGuid + "&position=" + startPos;
        console.log("Sending data to server: " + fullURL);
        var dtBefore = new Date();
        var response = await fetch(
            fullURL,
            {
                method: "POST",
                body: chunkBlob
            })
            .then(async response => {
                if (response.ok) {
                    var dtAfter = new Date();
                    var responseText = await response.text();
                    console.log("response_from_server: " + responseText);

                    // recalculate chunk size based on detected speed
                    var elapsedTime = (dtAfter.getTime() - dtBefore.getTime()) / 1000.0;
                    chunkSize = Math.floor(chunkSize / elapsedTime * targetRoundTrip);
                    if (chunkSize < chunkSizeMin) chunkSize = chunkSizeMin;
                    if (chunkSize > chunkSizeMax) chunkSize = chunkSizeMax;

                    // advance start position
                    startPos = endPos;
                    errorCounter = 0;
                    dirProgressText.innerText = "";
                } else {
                    // Errors like 404, 500, etc..
                    errorCounter++;
                    var errorMessage = "Error: " + response.status + " - " + response.statusText + " (" + errorCounter + ")";
                    dirProgressText.innerText = errorMessage;
                    console.log(errorMessage);
                }
            })
            .catch(error => {
                // connection error or other unknown error
                errorCounter++;
                dirProgressText.innerText = "Error: " + error.message + " (" + errorCounter + ")";
                console.log(error);
            });

        if (errorCounter > maxRetriesOnError) {
            console.log("errorCounter over limit --> exit");
            break;
        }
    }

    if (errorCounter == 0) {
        divProgressBarValue.style.width = divProgressBar.style.width;
        callbackOK();
    }
    else {
        callbackERR();
    }
}

