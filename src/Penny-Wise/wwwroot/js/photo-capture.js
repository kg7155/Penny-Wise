var video = document.querySelector("#video");
var constraints = { audio: false, video: { width: 320, height: 240 } }; 

navigator.mediaDevices.getUserMedia(constraints)
.then(function(mediaStream) {
  video.srcObject = mediaStream;
  video.onloadedmetadata = function(e) {
    video.play();
  };
})
.catch(function(err) { console.log(err.name + ": " + err.message); });

var canvas = document.getElementById("canvas-video");
var context = canvas.getContext("2d");

document.getElementById("snap").addEventListener("click", function() {
  video.style.display = "none";
  canvas.style.display = "block";
  context.drawImage(video, 0, 0, 320, 240);
});

document.getElementById("take-new-photo").addEventListener("click", function() {
  canvas.style.display = "none";
  video.style.display = "block";
});