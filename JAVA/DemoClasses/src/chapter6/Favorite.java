// A program from Chapter 6 of Sams Teach Yourself Java in 24 Hours
// by Rogers Cadenhead, http://www.java24hours.com/

package chapter6;

class Favorite {
     public static void main(String[] arguments) {
         // set up film information
         String favorite = "chainsaw";
         String guess = "pool cue";
         System.out.println("Is Fin's favorite weapon a "
             + guess + "?");
         System.out.println("Answer: " + favorite.equals(guess));
    }
}