package com.example.demo;

import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.ResponseStatus;

@ResponseStatus(code = HttpStatus.BAD_REQUEST, reason = " there is no enough ingredients")
public class NotEnougthIngredientsException extends RuntimeException{
}
