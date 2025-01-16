
function InitPage() {
    const video = document.getElementById('video-player');
    let initsec = 0;
    const url = `/api/video/initial-seconds/${video.dataset.folder}/${video.dataset.file}`;
    console.log("Fetching initial seconds from server...");
    fetch(url, { headers: { 'Content-Type': 'application/json' } })
        .then(response => response.text())
        .then(res => {
            if (parseInt(res) > 0) {
                video.currentTime = parseInt(res);
            }
            initsec = parseInt(res);
        })
        .catch(e => console.error(e));
    
    let timesChanged = 0;
    video.addEventListener("timeupdate", function (e) {
        if (timesChanged < 10) { // to do one request per minute
            timesChanged++;
            return;
        }
        timesChanged = 0;
        console.log("Sending time to server...");
        fetch(`${url}?time=${video.currentTime}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        }).catch(e => console.error(e));
    });

    // window.addEventListener("unload", function (e) {
    //     if (video.currentTime === 0 || video.currentTime < initsec) {
    //         return;
    //     }
    //     fetch(`${url}?time=${video.currentTime}`, {
    //         method: 'POST',
    //         headers: { 'Content-Type': 'application/json' }
    //     }).catch(e => console.error(e));
    // });
}