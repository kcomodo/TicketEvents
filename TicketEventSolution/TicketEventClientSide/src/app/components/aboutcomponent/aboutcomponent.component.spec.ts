import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AboutcomponentComponent } from './aboutcomponent.component';

describe('AboutcomponentComponent', () => {
  let component: AboutcomponentComponent;
  let fixture: ComponentFixture<AboutcomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AboutcomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AboutcomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
