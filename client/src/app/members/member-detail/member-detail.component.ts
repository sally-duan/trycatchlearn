import { createViewChild } from '@angular/compiler/src/core';
import { Component, OnInit, ViewChild } from '@angular/core';
<<<<<<< HEAD
import { ActivatedRoute } from '@angular/router';
=======
import { ActivatedRoute} from '@angular/router';
>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from '../../_models/message';
import { Member } from '../../_models/member';
import { MembersService } from '../../_services/members.service';
import { MessageService } from '../../_services/message.service';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
<<<<<<< HEAD
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  member: Member = {} as Member;
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  activeTab?: TabDirective;
  messages: Message[] = []


  constructor(private memberService: MembersService, private route: ActivatedRoute,
    private messageService: MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data['member']
    });
    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100,
      thumbnailsColumns: 4,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }]
    this.galleryImages = this.getImages()
=======
  @ViewChild('memberTabs', {static:true}) memberTabs?: TabsetComponent;
  member:Member ={} as Member;
  galleryOptions:NgxGalleryOptions[] =[];
  galleryImages : NgxGalleryImage[] =[];
  activeTab?: TabDirective;
  messages: Message[] |undefined;


  constructor(private memberService:MembersService, private route:ActivatedRoute,
     private messageService:MessageService) { }

  ngOnInit(): void {
    this.route.data.subscribe({
      next:data=>this.member=data['member']
    });
    this.route.queryParams.subscribe({
      next:params=>{
        params['tab']&& this.selectTab(params['tab'])
      }
    })
    this.galleryOptions =[{
      width:'500px',
      height:'500px',
      imagePercent:100,
      thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,
      preview:false
    }]   
    this.galleryImages = this.getImages()  
>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
  }

  getImages() {
    if (!this.member) return [];
    const imageUrls = [];
    for (const photo of this.member.photos) {
      imageUrls.push({
        small: photo.url,
        medium: photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }

  // loadMember()
  // {
  //   const username = this.route.snapshot.paramMap.get('username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next:member=>{       
<<<<<<< HEAD

=======
        
>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
  //       this.member = member;
  //       // this.galleryImages = this.getImages() 
  //      }
  //   });    
  // }

<<<<<<< HEAD
  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true
    }
  }



  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
=======
selectTab(heading:string)
{
  if (this.memberTabs){
    this.memberTabs.tabs.find(x=>x.heading===heading)!.active =true
  }
}



  loadMessages()
  {
    if (this.member)
    {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages =>this.messages = messages
>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
      })
    }
  }

<<<<<<< HEAD
  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages') {
=======
  onTabActivated(data:TabDirective)
  {
    this.activeTab = data;
    if (this.activeTab.heading ==='Messages')
    {
>>>>>>> ead028757c5a25c5ef9bbfbfdb8724eddc91f1dd
      this.loadMessages();
    }
  }

}
