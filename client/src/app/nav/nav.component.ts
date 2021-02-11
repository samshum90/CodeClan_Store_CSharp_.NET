import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  loggedIn: boolean = true;
  constructor() { }

  ngOnInit(): void {
  }

  login() {
    this.loggedIn = !this.loggedIn;
  }
  logout() {
    this.loggedIn = !this.loggedIn;
  }
}
