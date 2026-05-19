namespace BloggingPlatform.Application.DTOs.Common;
public class ApiResponse<T>{public bool Success{get;set;}=true;public string Message{get;set;}="";public T? Data{get;set;}public List<string> Errors{get;set;}=new();public static ApiResponse<T> Ok(T data,string msg="Success")=>new(){Data=data,Message=msg};}
public record PagedResult<T>(IEnumerable<T> Items,int Page,int PageSize,long Total);
