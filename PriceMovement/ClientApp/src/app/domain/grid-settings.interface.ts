import { State, DataResult } from '@progress/kendo-data-query';
import { ColumnSettings } from './column-settings.interface';

export interface GridSettings {
  state: State;
  columnsConfig: ColumnSettings[];
  filterable: boolean;
  groupable: boolean;
}
