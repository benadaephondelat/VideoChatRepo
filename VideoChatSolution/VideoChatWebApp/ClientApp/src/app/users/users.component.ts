import { Component, Inject, OnInit, Output } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient } from "@angular/common/http";

@Component({
    selector: "users",
    templateUrl: "./users.component.html",
    styleUrls: ['./users.component.css']
})

export class UsersComponent implements OnInit {
  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.http.get("api/Test/Test").subscribe((data: any) => {
    });
  }
}
