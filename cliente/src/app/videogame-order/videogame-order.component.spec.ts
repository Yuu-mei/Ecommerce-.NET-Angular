import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VideogameOrderComponent } from './videogame-order.component';

describe('VideogameOrderComponent', () => {
  let component: VideogameOrderComponent;
  let fixture: ComponentFixture<VideogameOrderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VideogameOrderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(VideogameOrderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
