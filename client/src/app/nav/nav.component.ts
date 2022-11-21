import { Component, OnInit } from '@angular/core';
import { AccountService } from './../_services/account.service';
import {BsDropdownModule}  from 'ngx-bootstrap/dropdown';
import { Router } from '@angular/router';
import {ToastrService} from 'ngx-toastr';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model:any ={};
  loggedIn:boolean;
  constructor(public accountService:AccountService, private router:Router, private toastr: ToastrService ) { }

  ngOnInit(): void {
  }

  login(){
    this.accountService.login(this.model).subscribe(response=>{
      this.router.navigateByUrl("/members")
      this.loggedIn = true;
    },
    error=>{
      console.log(error);
      this.toastr.error(error.error);
    })
  }


  
  logout(){   
    this.router.navigateByUrl("/")
    this.accountService.logout();
    this.loggedIn = false;    
  }

  getCurrentUser(){
    this.accountService.currentUser$.subscribe(user=>{
      this.loggedIn =!!user;
    }, error =>{
      console.log(error);
    })
  }
}
