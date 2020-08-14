using System;
using System.Collections.Generic;

namespace server.Dtos
{
  // public class GroupedDocCallThread {

  // 	public string ConfirmationNumber { get; set; }

  // 	public List<DocCallThread> Threads { get; set; }

  // }
  // public class DocCallThread {
  // 	public int Id { get; set; }
  // 	public string ConfirmationNumber { get; set; }
  // 	public DateTime CreatedAt { get; set; }
  // 	public string Comment { get; set; }
  // 	public string CallType { get; set; }
  // }
  public class DocCallThread
  {

    public int Id { get; set; }
    public string ConfirmationNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Comment { get; set; }
    public string CallType { get; set; }

    public List<DocCallThread> Threads { get; set; }

  }
  // public class DocCallThread {
  // 	public int Id { get; set; }
  // 	public string ConfirmationNumber { get; set; }
  // 	public DateTime CreatedAt { get; set; }
  // 	public string Comment { get; set; }
  // 	public string CallType { get; set; }
  // }
}