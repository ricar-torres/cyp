export const MenuRoles = {
  USERS: ['User.Read', 'User.Create', 'User.Update', 'User.Delete'],
  USERS_CREATE: ['User.Create'],
  USERS_UPDATE: ['User.Update'],
  USERS_DELETE: ['User.Delete'],

  FILES: ['File.Read', 'File.Create', 'File.Update', 'File.Delete'],
  FILES_CREATE: ['File.Create'],
  FILES_UPDATE: ['File.Update'],
  FILES_DELETE: ['File.Delete'],

  DOCUMENTS: [
    'Document.Read',
    'Document.Create',
    'Document.Update',
    'Document.Delete',
  ],
  DOCUMENTS_CREATE: ['Document.Create'],
  DOCUMENTS_UPDATE: ['Document.Update'],
  DOCUMENTS_DELETE: ['Document.Delete'],

  TASKS: ['Task.Read', 'Task.Create', 'Task.Update', 'Task.Delete'],
  TASKS_CREATE: ['Task.Create'],
  TASKS_UPDATE: ['Task.Update'],
  TASKS_DELETE: ['Task.Delete'],

  COMMENTS: [
    'Comment.Read',
    'Comment.Create',
    'Comment.Update',
    'Comment.Delete',
  ],
  COMMENTS_CREATE: ['Comment.Create'],
  COMMENTS_UPDATE: ['Comment.Update'],
  COMMENTS_DELETE: ['Comment.Delete'],

  DOCUMENT_TYPES: [
    'DocumentType.Read',
    'DocumentType.Create',
    'DocumentType.Update',
    'DocumentType.Delete',
  ],
  DOCUMENT_TYPES_CREATE: ['DocumentType.Create'],
  DOCUMENT_TYPES_UPDATE: ['DocumentType.Update'],
  DOCUMENT_TYPES_DELETE: ['DocumentType.Delete'],
};

export interface PERMISSION {
  read: boolean;
  create: boolean;
  update: boolean;
  delete: boolean;
  upload: boolean;
}

export const TaskStatus = {
  sent: 'SENT',
  inbox: 'INBOX',
  completed: 'COMPLETED',
  archived: 'ARCHIVED',
  cancelled: 'CANCELLED',
};
