import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdataComponent } from './updata.component';

describe('UpdataComponent', () => {
  let component: UpdataComponent;
  let fixture: ComponentFixture<UpdataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdataComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
