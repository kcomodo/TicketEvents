import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventscomponentComponent } from './eventscomponent.component';

describe('EventscomponentComponent', () => {
  let component: EventscomponentComponent;
  let fixture: ComponentFixture<EventscomponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [EventscomponentComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EventscomponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
