
async function SendFile(apiBaseURL, callbackOK, callbackERR) {
    try {
        // Config params
        const targetRoundTrip = 2;  // seconds
        const chunkSizeMin = 10 * 1024;
        const chunkSizeMax = 2 * 1024 * 1024;
        const maxRetriesOnError = 3;

        // flag for stopping execution
        let stop = false;

        // UI elements
        const uploadGuid = document.getElementById("UploadGuid").value;
        const fileInput = document.getElementById("FileInput");
        const divProgress = document.getElementById("divProgress");
        const divProgressBar = document.getElementById("divProgressBar");
        const divProgressBarValue = document.getElementById("divProgressBarValue");
        const dirProgressText = document.getElementById("dirProgressText");
        const buttonSubmit = document.getElementById("buttonSubmit");

        const buttonStop = document.getElementById("buttonStop");
        buttonStop.onclick = () => {
            stop = true;
            buttonSubmit.style.visibility = "visible";
            divProgress.style.visibility = "hidden";
            divProgressBarValue.style.width = "0px";
            buttonStop.style.visibility = "hidden";
            return false;
        } 

        // exit if no file selected
        if (fileInput.files.length == 0)
            return;

        // init UI
        buttonSubmit.style.visibility = "hidden";
        buttonStop.style.visibility = "visible";
        divProgress.style.visibility = "visible";
        divProgressBarValue.style.width = "0px";
        dirProgressText.innerText = "";

        // Process file
        const sourceFile = fileInput.files[0];
        let chunkSize = chunkSizeMin;
        let endPos = chunkSize;
        let errorCounter = 0;
        let progressBarWidth = parseInt(divProgressBar.style.width, 10);

        // get start position
        let startPos;
        const getFileLengthReponse = await fetch(apiBaseURL + "?action=getFileLength&uploadguid=" + uploadGuid, { method: "GET" });

        if (getFileLengthReponse && getFileLengthReponse.ok) {
            startPos = parseInt(await getFileLengthReponse.text(), 10);
            if (!startPos || startPos < 0) {
                startPos = 0;
            }
        }
        else {
            const getFileLengthErrorMessage = getErrorMessage(getFileLengthReponse.status, getFileLengthReponse.statusText, errorCounter);
            dirProgressText.innerText = getFileLengthErrorMessage;
            console.log(getFileLengthErrorMessage);
            callbackERR();
            return; // exit
        }


        // upload chunks cycle
        while (startPos < sourceFile.size) {

            console.log("_CHUNK_");

            // progress bar update
            divProgressBarValue.style.width = Math.floor(startPos * progressBarWidth / sourceFile.size) + "px";

            // get chunk from file
            console.log("Read from file: startPos=" + startPos + " chunkSize=" + chunkSize);
            endPos = startPos + chunkSize;
            const chunkBlob = sourceFile.slice(startPos, endPos);

            // Send data                       
            const fullURL = apiBaseURL + "?action=upload&uploadguid=" + uploadGuid + "&position=" + startPos;
            console.log("Sending data to server: " + fullURL);
            const dtBefore = new Date();

            try {
                const uploadChunkResponse = await fetch(fullURL, { method: "POST", body: chunkBlob })

                if (uploadChunkResponse && uploadChunkResponse.ok) {
                    const dtAfter = new Date();
                    const responseText = await uploadChunkResponse.text();
                    console.log("response_from_server: " + responseText);

                    // recalculate chunk size based on detected speed
                    const elapsedTime = (dtAfter.getTime() - dtBefore.getTime()) / 1000.0;
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
                    const uploadChunkErrorMessage = getErrorMessage(response.status, response.statusText, errorCounter);
                    dirProgressText.innerText = uploadChunkErrorMessage;
                    console.log(uploadChunkErrorMessage);
                }
            }
            catch (uploadChunkErr) {
                // connection error or other unknown error
                errorCounter++;
                dirProgressText.innerText = "Error: " + error.message + " (" + errorCounter + ")";
                console.log(error);
            }

            if (errorCounter > maxRetriesOnError) {
                console.log("errorCounter over limit --> exit");
                break;
            }

            if (stop) {
                console.log("stop flag --> exit");
                break;
            }
        }

        if (!stop) {
            if (errorCounter == 0) {
                divProgressBarValue.style.width = divProgressBar.style.width;
                callbackOK();
            }
            else {
                callbackERR();
            }
        }
    }
    catch (err) {
        console.log(err);
        callbackERR();
    }
}

function getErrorMessage(status, statusText, errorCounter) {
    return "Error: " + status + " - " + statusText + " (" + errorCounter + ")";
}
