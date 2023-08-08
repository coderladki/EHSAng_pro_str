import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmailMasterComponent } from './email-master.component';

describe('EmailMasterComponent', () => {
  let component: EmailMasterComponent;
  let fixture: ComponentFixture<EmailMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EmailMasterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmailMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
