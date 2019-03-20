import { Injectable } from '@angular/core';

import { GridSettings } from '../domain/grid-settings.interface';
import { Settings } from '../domain/settings';

@Injectable()
export class StatePersistingService {

  public getGridSettings<T>(token: string): T {
    const settings = localStorage.getItem(token);
    return settings ? JSON.parse(settings) : settings;
  }

  public setGridSettings<T>(token: string, gridConfig: GridSettings): void {
    localStorage.setItem(token, JSON.stringify(gridConfig));
  }

  public getDataSettings<T>(token: string): T {
    const settings = localStorage.getItem(token);
    return settings ? JSON.parse(settings) : settings;
  }

  public setDataSettings<T>(token: string, dataSettings: Settings): void {
    localStorage.setItem(token, JSON.stringify(dataSettings));
  }
}
