export interface IReadApplicationResponse {
  clientId: string;
  callbackUri: string;
  name: string;
  isDeleted: boolean;
  deletedOnUtc: string | null;
  deletedBy: string | null;
  createdOnUtc: string;
  createdBy: string | null;
  modifiedOnUtc: string | null;
  modifiedBy: string | null;
}
