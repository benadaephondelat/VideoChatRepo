import { Component, Inject, OnInit, Output, OnDestroy } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { ActivatedRoute } from "@angular/router";
import { ImageMessage } from "../../models/imagemessage";

@Component({
    selector: "private-chat",
    templateUrl: "./private-chat.component.html",
    styleUrls: ['./private-chat.component.css'],
})

export class PrivateChatComponent implements OnInit {
  private user: string;
  private currentUser: string;
  private params: any;

  public hubConnection: HubConnection;
  private nick = '';
  private message = '';
  private messages: string[] = [];
  private images: ImageMessage[] = [];

  selectedFile: File;

  constructor(private activeRoute: ActivatedRoute, private httpClient: HttpClient) {
    this.activeRoute.params.subscribe(params => this.params = params);
  }

  ngOnInit(): void {
    this.nick = this.params['user2'];
  
    this.hubConnection = new HubConnectionBuilder().withUrl('/chat').build();

    this.hubConnection.start()
                      .then(() => console.log('Connection started!'))
                      .catch(err => console.log('Error while establishing connection :('));

    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string) => {
      const text = `${nick}: ${receivedMessage}`;
      this.messages.push(text);
    });

    this.hubConnection.on('imageMessage', (username: string, data: ImageMessage) => {
      if (this.nick.toLowerCase() != username.toLowerCase()) {
        this.images.push(data);
      }
    });
  }

  onFileChanged(event) {
    this.selectedFile = event.target.files[0]
  }

  onUpload() {
    const uploadData = new FormData();
    uploadData.append('myFile', this.selectedFile, this.selectedFile.name);

    this.httpClient.post('https://localhost:44319/api/FileUpload/files', uploadData).subscribe(res => {
      console.dir(res);
    }, error => console.log(error));
  }

  public sendMessage(): void {
    this.hubConnection.invoke('sendToAll', this.nick, this.message)
                      .catch(err => console.error(err));
  }
}
