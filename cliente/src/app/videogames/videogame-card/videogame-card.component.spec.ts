import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideogameCardComponent } from './videogame-card.component';

describe('VideogameCardComponent', () => {
  let component: VideogameCardComponent;
  let fixture: ComponentFixture<VideogameCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideogameCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideogameCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
