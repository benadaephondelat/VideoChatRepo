import { Component, Inject, OnInit, Output, OnDestroy } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
    selector: "users",
    templateUrl: "./users.component.html",
    styleUrls: ['./users.component.css'],
})

export class UsersComponent implements OnInit, OnDestroy {
  public hubConnection: HubConnection;
  private users: string[];
  private currentUser: string;

  constructor(private http: HttpClient, private router: Router) {
    this.users = new Array<string>();
  }

  ngOnInit(): void {
    if (this.users == null) {
      this.users = new Array<string>();
    }

    this.currentUser = JSON.parse(localStorage.getItem('auth'))['username'].toString();

    this.http.get("api/User/GetUsers").subscribe((data: any) => {
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

      this.hubConnection.on('joinRoom', (currentUser: string, otherUser: string) => {
        this.joinRoom(currentUser, otherUser);
      });

      this.hubConnection.invoke('addUserToChatRoom', this.currentUser);

    }).catch(err => console.log('Error while establishing connection :('));
  }

  ngOnDestroy(): void {
    var authkey = localStorage.getItem('auth');

    if (typeof authkey !== 'undefined') {
      let username = JSON.parse(authkey)['username'].toString();

      this.hubConnection.invoke('removeUserFromChatRoom', username);
    }

    this.hubConnection.stop();
  }

  public startChat(username: string) {
    if (username == this.currentUser) {
      alert('can not start chat with self :)');
    } else {

      let currentUserUsername = this.currentUser;

      this.hubConnection.invoke('removeUserFromChatRoom', this.currentUser);
      this.hubConnection.invoke('removeUserFromChatRoom', username);
      this.hubConnection.invoke('joinRoom', this.currentUser, username);

      this.router.navigate(['/private-chat', {
        user1: username,
        user2: currentUserUsername
      }]);
    }
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

  public joinRoom(currentUser: string, otherUser: string): void {
    if (this.currentUser == otherUser) {

      this.hubConnection.invoke('removeUserFromChatRoom', currentUser);

      this.router.navigate(['/private-chat', {
        user1: currentUser,
        user2: otherUser
      }]);
    }
  }

  public isNotCurrentUser(user: string): boolean {
    if (this.currentUser !== user) {
      return true;
    }

    return false;
  }
}
