import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { take } from 'rxjs/operators';
import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { MembersService } from '../../_services/members.service';
import { AccountService } from './../../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';


@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm|undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any) {
    if (this.editForm?.dirty) {
       $event.returnValue = true;     
    }   
  }
// @HostListener('window:unload', ['$event'])
// unloadHandler($event:any) {
//     this.PostCall();
// }

// @HostListener('window:beforeunload', ['$event'])
// beforeUnloadHander($event:any) {
//     return false;
// }

// PostCall() {
//     console.log('PostCall');
// }
member:Member |undefined;
user:User|null =null;
constructor(private accountService:AccountService, private memberService:MembersService, private toastr:ToastrService ) {
this.accountService.currentUser$.pipe(take(1)).subscribe(
 { next:user=>this.user = user}
)
}

ngOnInit(): void {
    this.loadMember();
}
loadMember(){
if(!this.user) return;
this.memberService.getMember(this.user.username).subscribe({
  next:member=>this.member = member
})}

updateMember(){
  console.log(this.member);
  console.log('hello');
  this.memberService.updateMember(this.editForm?.value).subscribe({
  next:_=>{
  this.toastr.success("profile updated successfully.");
  this.editForm?.reset(this.member);
}
   }
  )
  
}

}
