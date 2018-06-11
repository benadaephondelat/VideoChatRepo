import { Component, Inject, OnInit, Output, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MethodNames } from '../interfaces/usersHubMethodNames';
import { ActivatedRoute } from "@angular/router";

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


  constructor(private activeRoute: ActivatedRoute) {
    this.activeRoute.params.subscribe(params => this.params = params);
  }

  ngOnInit(): void {
    this.nick = this.params['user2'];

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
