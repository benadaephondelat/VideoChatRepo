import { Component, Inject, OnInit, Output, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { MethodNames } from '../interfaces/usersHubMethodNames';

@Component({
    selector: "users",
    templateUrl: "./users.component.html",
    styleUrls: ['./users.component.css']
})

export class UsersComponent implements OnInit, OnDestroy {
  constructor(private http: HttpClient) {
    this.users = new Array<string>();
  }

  public hubConnection: HubConnection;
  private users: string[];

  ngOnInit(): void {

    if (this.users == null) {
      this.users = new Array<string>();
    }

    this.http.get("api/Users/GetUsers").subscribe((data: any) => {
      this.users = data;
    });

    this.hubConnection = new HubConnectionBuilder().withUrl('/users').build();

    this.hubConnection.start().then(() => {
        this.hubConnection.on('addUserToChatRoom', (user: string) => {
          this.addUserToChatRoom(user);
        });

        this.hubConnection.on('removeUserFromChatRoom', (user: string) => {
          this.removeUserFromChatRoom(user);
        });

        var username = JSON.parse(localStorage.getItem('auth'))['username'].toString();

        this.hubConnection.invoke('addUserToChatRoom', username);

      }).catch(err => console.log('Error while establishing connection :('));
  }

  ngOnDestroy(): void {
    var authkey = localStorage.getItem('auth');

    if (typeof authkey !== 'undefined') {
      var username = JSON.parse(authkey)['username'].toString();

      this.hubConnection.invoke('removeUserFromChatRoom', username);
    }

    this.hubConnection.stop();
  }

  public addUserToChatRoom(username: string): void {
    var index: number = this.users.indexOf(username, 0);

    if (index === -1) {
      this.users.push(username);
    }
  }

  public removeUserFromChatRoom(username: string): void {
    var index: number = this.users.indexOf(username, 0);

    if (index > -1) {
      this.users.splice(index, 1);
    }
  }
}
