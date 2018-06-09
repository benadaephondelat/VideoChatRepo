import { Component, Inject, OnInit, Output } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: "users",
    templateUrl: "./users.component.html",
    styleUrls: ['./users.component.css']
})

export class UsersComponent implements OnInit {
  constructor(private http: HttpClient) {

  }

  public hubConnection: HubConnection;
  private users: string[];
  private nick = '';
  private message = '';
  private messages: string[] = [];

  ngOnInit(): void {
    this.http.get("api/Users/GetUsers").subscribe((data: any) => {
      this.users = data;
    });

    this.nick = window.prompt('Your name:', 'John');

    this.hubConnection = new HubConnectionBuilder().withUrl('/chat').build();

    this.hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));

    this.hubConnection.on('sendToAll', (nick: string, receivedMessage: string) => {
      const text = `${nick}: ${receivedMessage}`;
      this.messages.push(text);
    });

  }

  public sendMessage(): void {
    this.hubConnection
      .invoke('sendToAll', this.nick, this.message)
      .catch(err => console.error(err));
  }
}
