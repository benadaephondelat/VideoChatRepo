import { Component, AfterViewInit } from '@angular/core';
import * as WebRTC from '../../../assets/webrtc.js';

@Component({
  selector: 'app-video',
  templateUrl: './video.component.html',
  styleUrls: ['./video.component.css']
})
export class VideoComponent implements AfterViewInit {

  constructor() { }

  ngAfterViewInit(): void {
    var webrtc = new WebRTC({
      localVideoEl: 'localVideo',
      remoteVideosEl: 'remotesVideos',
      autoRequestMedia: true
    });

    webrtc.on('readyToCall', function () {
      webrtc.joinRoom('your awesome room name');
    });
  }
}
