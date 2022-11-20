import { Component, OnInit} from '@angular/core';

import { Input, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccountService } from './../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Input() usersFromParentComponent:any;
  @Output() cancelRegistraton = new EventEmitter();
  model:any ={}

  constructor(private http:HttpClient, private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register(){
  this.accountService.register(this.model).subscribe(
    response=>{
      console.log(response);
      this.cancel();
    }
  )
  
  }

  cancel(){
    this.cancelRegistraton.emit(false);
  }

}
